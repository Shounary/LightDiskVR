using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    [Header("Internal References")]
    public Enemy enemy;

    private BTNode root;
    private bool isExecuting;
    private Coroutine behavior;

    public BTNode Root { get { return root;} }

    private void Start() {
        isExecuting = false;

        // Dodge Branch
        DownDodgeAction downDodgeLeaf = new DownDodgeAction(this, enemy);
        InDangerHighDaemon downDodgeDaemon = new InDangerHighDaemon(this, downDodgeLeaf, enemy);

        JumpDodgeAction jumpDodgeLeaf = new JumpDodgeAction(this, enemy);
        InDangerLowDaemon jumpDodgeDaemon = new InDangerLowDaemon(this, jumpDodgeLeaf, enemy);

        // { Right/Left Side Dodge
        RightSideDodgeAction rightSideDodgeLeaf = new RightSideDodgeAction(this, enemy);
        InDangerLeftDaemon leftSideDodgeDaemon = new InDangerLeftDaemon(this, rightSideDodgeLeaf, enemy);

        LeftSideDodgeAction leftSideDodgeLeaf = new LeftSideDodgeAction(this, enemy);
        InDangerRightDaemon rightSideDodgeDaemon = new InDangerRightDaemon(this, leftSideDodgeLeaf, enemy);

        BTSelector sideDodgeSelector = new BTSelector(this, new BTNode[] { leftSideDodgeDaemon, rightSideDodgeDaemon });
        InDangerMiddleDaemon sideDodgeDaemon = new InDangerMiddleDaemon(this, sideDodgeSelector, enemy);


        // }

        BTSelector dodgeBranch = new BTSelector(this, new BTNode[] { downDodgeDaemon, jumpDodgeDaemon, sideDodgeDaemon });
        NotInRecoveryDaemon dodgeBranchDaemon = new NotInRecoveryDaemon(this, dodgeBranch, enemy);


        // Recovery Branch
        RecoveryAction recoveryLeaf = new RecoveryAction(this, enemy);
        InRecoveryDaemon recoveryDaemon = new InRecoveryDaemon(this, recoveryLeaf, enemy);

        // Zone Move Branch
        Move2DAction move2DLeaf = new Move2DAction(this, enemy);
        NotInZoneDaemon notInZoneMoveDaemon = new NotInZoneDaemon(this, move2DLeaf, enemy);
        NotInDangerDaemon notInDangerMoveDaemon = new NotInDangerDaemon(this, notInZoneMoveDaemon, enemy);
        NotInRecoveryDaemon notInRecoveryMoveDaemon = new NotInRecoveryDaemon(this, notInDangerMoveDaemon, enemy);

        // Attack Branch
        SummonAction summonActionLeaf = new SummonAction(this, enemy);
        HasNoDiskDaemon noDiskSummonActionDaemon = new HasNoDiskDaemon(this, summonActionLeaf, enemy);

        ThrowAction throwActionLeaf = new ThrowAction(this, enemy);
        HasDiskDaemon diskThrowActionDaemon = new HasDiskDaemon(this, throwActionLeaf, enemy);

        BTSelector attackBranch = new BTSelector(this, new BTNode[] { noDiskSummonActionDaemon, diskThrowActionDaemon });

        NotInRecoveryDaemon notInRecoveryAttackDaemon = new NotInRecoveryDaemon(this, attackBranch, enemy);



        BTSelector mainSelector = new BTSelector(this, new BTNode[] {recoveryDaemon, dodgeBranchDaemon, notInRecoveryAttackDaemon, notInRecoveryMoveDaemon});
        //BTSelector mainSelector = new BTSelector(this, new BTNode[] { recoveryDaemon, dodgeBranchDaemon, notInDangerMoveDaemon});
        //BTSelector mainSelector = new BTSelector(this, new BTNode[] { });
        root = mainSelector;
    }

    private void Update() {
        if (!isExecuting) {
            isExecuting = true;
            behavior = StartCoroutine(ExecuteBehavior());
        }
    }

    private IEnumerator ExecuteBehavior() {
        BTNode.Response response = Root.Execute();
        while (response == BTNode.Response.Running) {
            Debug.Log("Root response: " + response);
            yield return null;
            response = Root.Execute();
        }
        isExecuting = false;
        Debug.Log("Execution finished with: " + response);
    }
}
