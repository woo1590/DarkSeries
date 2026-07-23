using System.Collections;
using UnityEngine;

public class SwordMasterAttackController : MonoBehaviour
{
    public int slashComboIndex { get; private set; }
    public bool bufferSlashCombo { get; private set; }

    /* Slash Attack */
    public void BufferSlash()
    {
        bufferSlashCombo = true;
    }

    public void IncreaseCombo()
    {
        slashComboIndex++;

        if (slashComboIndex > 3)
            slashComboIndex = 0;
    }

    public bool ConsumeBuffer()
    {
        if (!bufferSlashCombo)
            return false;

        bufferSlashCombo = false;
        return true;
    }

    public void ResetCombo()
    {
        bufferSlashCombo = false;
        slashComboIndex = 0;
    }
}
