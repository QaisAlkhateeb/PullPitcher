using PullPitcher.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace PullPitcher.Permissions;

public class PullPitcherPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(PullPitcherPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(PullPitcherPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PullPitcherResource>(name);
    }
}
