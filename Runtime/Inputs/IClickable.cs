using UnityEngine;

namespace Drolegames.Inputs
{
    public interface IClickable
    {
        void OnClick(Vector3 position);
        void OnSecondaryClick(Vector3 position);
    }
}