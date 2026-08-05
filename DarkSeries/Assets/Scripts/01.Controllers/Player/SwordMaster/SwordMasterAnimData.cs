using System;
using UnityEngine;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class SwordMasterAnimData
{
    [SerializeField] private string idleParamName = "Idle";
    [SerializeField] private string walkParamName = "Walk";
    [SerializeField] private string runParamName = "Run";
    [SerializeField] private string runFastParamName = "RunFast";
    [SerializeField] private string attackParamName = "Attack";
    [SerializeField] private string isGroundParamName = "IsGround";
    [SerializeField] private string startFallParamName = "StartFall";
    [SerializeField] private string fallParamName = "Fall";
    [SerializeField] private string landParamName = "Land";
    [SerializeField] private string crouchParamName = "Crouch";

    public int idleParamHash {  get; private set; }
    public int walkParamHash { get; private set ; }
    public int runParamHash { get; private set ; }
    public int runFastParamHash { get;private set ; }   
    public int attackParamHash { get; private set ; }
    public int isGroundParamHash { get; private set; }
    public int startFallParamHash { get; private set ; } 
    public int fallParamHash { get; private set ; } 
    public int landParamHash { get; private set ; } 
    public int crouchParamHash { get; private set ; }

    public void Initialize()
    {
        idleParamHash = Animator.StringToHash(idleParamName);
        walkParamHash = Animator.StringToHash(walkParamName);
        runParamHash = Animator.StringToHash(runParamName);
        runFastParamHash = Animator.StringToHash(runFastParamName);
        attackParamHash = Animator.StringToHash(attackParamName);
        isGroundParamHash = Animator.StringToHash(isGroundParamName);
        startFallParamHash = Animator.StringToHash(startFallParamName);
        fallParamHash = Animator.StringToHash(fallParamName);
        landParamHash = Animator.StringToHash(landParamName);
        crouchParamHash = Animator.StringToHash(crouchParamName);
    }
}
