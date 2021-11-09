using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ScreamJam
{
    public class NumberScript : MonoBehaviour
    {
        [SerializeField] int a;
        [SerializeField] int b;
        [SerializeField] int c;

        int _smallest;
        int _medium;
        int _largest;

        [Button("Run Function")]
        void ArrangeNumbersInOrder()
        {
            if (a > b && a > c)
            {
                _largest = a;

                if (b > c)
                {
                    _medium = b;
                    _smallest = c;
                }
                else if (c > b)
                {
                    _medium = c;
                    _smallest = b;
                }
            }
            else if (b > a && b > c)
            {
                _largest = b;
                if (a > c)
                {
                    _medium = a;
                    _smallest = c;
                }
                else if (c > a)
                {
                    _medium = c;
                    _smallest = a;
                }
            }
            else if (c > a && c > b)
            {
                _largest = c;
                if (a > b)
                {
                    _medium = a;
                    _smallest = b;
                }
                else if (b > a)
                {
                    _medium = b;
                    _smallest = a;
                }
            }
            Debug.Log($"{_largest}, {_medium}, {_smallest}");
        }
        [SerializeField] GameEvent _event;
        [Button("RaiseEvent")]
        void RaiseEvent()
        {
            _event.Raise();
        }



    }
}
