using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages
{
    public interface IPageBase 
    {
        bool IsProcessing { get; set; }
    }
}
