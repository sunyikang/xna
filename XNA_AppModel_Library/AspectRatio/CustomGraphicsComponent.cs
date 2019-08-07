#region File Description
//-----------------------------------------------------------------------------
// CustomGraphicsComponent.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AspectRatio
{
    public class CustomGraphicsDeviceManager : GraphicsDeviceManager
    {
        public CustomGraphicsDeviceManager( Game game )
            : base( game )
        {
        }

        private bool isWideScreenOnly;
        public bool IsWideScreenOnly
        {
            get { return isWideScreenOnly; }
            set { isWideScreenOnly = value; }
        }

        static float WideScreenRatio = 1.6f; //1.77777779f;

        protected override void RankDevices( List<GraphicsDeviceInformation> foundDevices )
        {
            base.RankDevices( foundDevices );
            if (IsWideScreenOnly)
            {
                for (int i = 0; i < foundDevices.Count; )
                {
                    PresentationParameters pp = foundDevices[i].PresentationParameters;
                    if (pp.IsFullScreen == true)
                    {
                        float aspectRatio = (float)(pp.BackBufferWidth) / (float)(pp.BackBufferHeight);

                        // If the device does not have a widescreen aspect ratio, remove it.
                        if (aspectRatio < WideScreenRatio) 
                        { 
                            foundDevices.RemoveAt( i ); 
                        }
                        else { i++; }
                    }
                    else i++;
                }
            }
        }
    }

}


