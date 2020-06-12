using UnityEngine;
using UnityEngine.Experimental.Input;

namespace Prosthetic.Scripts
{
    public interface IRotationInput
    {
        Vector3 RotationDirection { get; set; }
    }
}