using UnityEngine;
using UnityEngine.Experimental.Input;

namespace Prosthetic.Scripts
{
    public interface IInteractInput
    {
        bool IsPressingInteract { get; }
    }
}