﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBatmanGame.GameObjects.Factories
{
    public interface IGameObjectFactory
    {
        GameObject Get(int left, int top);
    }
}
