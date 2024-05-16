using Volo.Abp.Settings;

namespace PullPitcher.Settings;

public class PullPitcherSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(PullPitcherSettings.MySetting1));
    }
}
