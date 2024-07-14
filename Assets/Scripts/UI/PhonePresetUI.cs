using TMPro;
using UnityEngine;

namespace TuioSimulator.UI
{
    public class PhonePresetUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;

        public void SetText(string name)
        {
            _name.text = name;
        }
    }
}