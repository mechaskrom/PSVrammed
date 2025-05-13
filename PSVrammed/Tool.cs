using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSVrammed
{
    enum Tool
    {
        None,
        Zoom, //Zoom at mouse position.
        Situate, //Situate marker with mouse.
        Inspect, //Show info about pixel at mouse position.
        Edit, //Very similar to move marker.
    }
}
