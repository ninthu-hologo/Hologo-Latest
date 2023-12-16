using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDateComparison<T>
{
    bool dateCompare(T other);
}
