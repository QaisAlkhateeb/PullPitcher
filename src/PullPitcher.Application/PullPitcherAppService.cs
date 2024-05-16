using System;
using System.Collections.Generic;
using System.Text;
using PullPitcher.Localization;
using Volo.Abp.Application.Services;

namespace PullPitcher;

/* Inherit your application services from this class.
 */
public abstract class PullPitcherAppService : ApplicationService
{
    protected PullPitcherAppService()
    {
        LocalizationResource = typeof(PullPitcherResource);
    }
}
