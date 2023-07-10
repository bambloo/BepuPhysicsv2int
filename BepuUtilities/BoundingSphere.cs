﻿using System;
using System.Collections.Generic;
using System.Linq;
using BepuUtilities.Numerics;
using System.Text;

namespace BepuUtilities
{    
    /// <summary>
    /// Provides XNA-like bounding sphere functionality.
    /// </summary>
    public struct BoundingSphere
    {
        /// <summary>
        /// Location of the center of the sphere.
        /// </summary>
        public Vector3 Center;
        /// <summary>
        /// Radius of the sphere.
        /// </summary>
        public Number Radius;

        /// <summary>
        /// Constructs a new bounding sphere.
        /// </summary>
        /// <param name="center">Location of the center of the sphere.</param>
        /// <param name="radius">Radius of the sphere.</param>
        public BoundingSphere(Vector3 center, Number radius)
        {
            this.Center = center;
            this.Radius = radius;
        }
    }
}
