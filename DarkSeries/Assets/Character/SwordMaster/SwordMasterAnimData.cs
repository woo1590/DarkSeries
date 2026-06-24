using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class SwordMasterAnimData
{
    [SerializeField] private string idleParamName = "Idle";
    [SerializeField] private string walkParamName = "Walk";
    [SerializeField] private string runParamName = "Run";
    [SerializeField] private string runFastParamName = "RunFast";

    public int idleParamHash {  get; private set; }
    public int walkParamHash { get; private set ; }
    public int runParamHash { get; private set ; }
    public int runFastParamHash { get;private set ; }   

    public void Initialize()
    {
        idleParamHash = Animator.StringToHash(idleParamName);
        walkParamHash = Animator.StringToHash(walkParamName);
        runParamHash = Animator.StringToHash(runParamName);
        runFastParamHash = Animator.StringToHash(runFastParamName);
    }
}