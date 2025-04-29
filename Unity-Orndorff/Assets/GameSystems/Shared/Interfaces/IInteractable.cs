/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Systems
* FILE NAME       : IInteractable.cs
* DESCRIPTION     : Interface for interactable items
*                    
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/04/08      Akram Taghavi-Burris      Created Interface
* 
*
/******************************************************************/

using UnityEngine;

namespace GameSystems.Shared.Interfaces
{
    public interface IInteractable
    {
         public void OnInteract(GameObject interactor);
         
    }//end IInteratable
    
}//end Namespace
