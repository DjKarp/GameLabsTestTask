using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotModule : Slot
{

    protected List<ModuleDesingSO> m_ModuleDesingSO = new List<ModuleDesingSO>();
    protected ModuleDesingSO currentModule;
        
    protected int moduleSOCount;



    protected override void Awake()
    {

        base.Awake();

        moduleSOCount = GameManager.Instance.moduleCountSO;

        for (int i = 1; i <= moduleSOCount; i++)
        {

            ModuleDesingSO tempDesingSO = Resources.Load("DesingSO/Module/Modul_0" + i) as ModuleDesingSO;
            if (tempDesingSO != null) m_ModuleDesingSO.Add(tempDesingSO);

        }

    }

    public override void ActivateSlot()
    {

        base.ActivateSlot();

        GameManager.Instance.choiseNumberModule = Mathf.Clamp(GameManager.Instance.choiseNumberModule, 0, m_ModuleDesingSO.Count);
        currentModule = m_ModuleDesingSO[GameManager.Instance.choiseNumberModule];

        m_Pawn.AddModule(this);

        switch (currentModule.CurrentRarityModul)
        {

            case ModuleDesingSO.RarityModul.standart:
                m_MeshRenderer.material = blueMaterial;
                break;

            case ModuleDesingSO.RarityModul.yellow:
                m_MeshRenderer.material = yellowMaterial;
                break;

            case ModuleDesingSO.RarityModul.purple:
                m_MeshRenderer.material = purpleMaterial;
                break;

        }

    }

    public ModuleDesingSO GetCurrentModule()
    {

        return currentModule;

    }

    public override void GreenPlaceModule(bool isOn)
    {

        transparentBoxPlace.SetActive(isOn);

    }

}
