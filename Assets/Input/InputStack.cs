using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputStack
{
    private List<BufferedInput> inputs { get; set; } = new List<BufferedInput>();

    public void Remove(InputAction byInputAction)
    {
        inputs.RemoveAll(bi => bi.MappedAction == byInputAction);
    }

    public void Add(BufferedInput toAdd)
    {
        // Already in the list? Ignore instead of adding
        if (inputs.Any((bi) => bi.MappedAction == toAdd.MappedAction))
        {
            return;
        }

        inputs.Add(toAdd);
    }

    public BufferedInput CurrentInput
    {
        get
        {
            if (inputs.Any())
            {
                return inputs.Last();
            }

            return null;
        }
    }
}
