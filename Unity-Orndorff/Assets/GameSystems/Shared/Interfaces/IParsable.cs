/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Systems
* FILE NAME       : IParseable.cs
* DESCRIPTION     : Interface for parsable data.
*                    
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/04/05      Akram Taghavi-Burris      Created Interface
* 
*
/******************************************************************/

namespace GameSystems.Shared.Interfaces
{
    public interface IParsable
    {
        void Parse(string[] fields);
    }
}
