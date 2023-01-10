// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelloActivity.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>
//     A sample activity
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;

using Microsoft.Azure.Cis.Activities.Contracts;

namespace $safeprojectname$
{
    /// <summary>
    /// A simple activity which showcases IActivityContext. Accesses Input Settings,
    /// Global Settings, Activity Settings, and PersistedActivity Settings.
    /// </summary>
    public sealed class HelloActivity : Activity
{
    /// <summary>
    /// Activity execution body
    /// </summary>
    /// <param name="context">Activity context</param>
    public override void Execute(IActivityContext context)
    {
        context.TraceStart("Start executing {0}", context.JobDisplayName);

        string message;
        if (!context.InputSettings.TryGetValue("Message", out message))
        {
            context.TraceError("Input Message not found");
        }

        context.TraceInfo(message);
        context.TraceInfo("Print GlobalSettings:");
        foreach (KeyValuePair<string, string> setting in context.GlobalSettings)
        {
            context.TraceInfo("Name = {0}; Value = {1}", setting.Key, setting.Value);
        }

        context.TraceInfo("Print HelloActivitySettings:");
        foreach (IActivitySettings activitySetting in context.ComponentSettings.ActivitiesSettingsCollection)
        {
            foreach (IEnvironmentSetting setting in activitySetting.GenericSettingsCollection)
            {
                context.TraceInfo("Name = {0}; Value = {1}", setting.Name, setting.Value);
            }
        }

        context.TraceInfo("Print JobId = {0}", context.JobId);
        context.TraceInfo("Print JobDisplayName = {0}", context.JobDisplayName);
        context.TraceInfo("Print ProjectRoot = {0}", context.ProjectRoot);
        context.TraceInfo("Print ScratchRoot = {0}", context.ScratchRoot);

        string valueOut;
        if (!context.ActivityPersistedSettings.TryGetValue("TestValue", out valueOut))
        {
            context.TraceInfo("Unable to get TestValue from activitySettings");
        }

        if (!context.ActivityPersistedSettings.ContainsKey("TestValue2"))
        {
            context.ActivityPersistedSettings.Add("TestValue2", "ThisIsTestData");
            context.TraceInfo("TestValue2 added to ActivityPersistedSettings");
        }
        else
        {
            context.TraceInfo("TestValue2 not added to ActivityPersistedSettings");
        }

        // example how to add global settings from activity
        if (!context.GlobalSettings.TryGetValue("TestGlobalValue", out valueOut))
        {
            context.TraceInfo("TestGlobalValue is not set. adding it");
        }
        context.GlobalSettings["TestGlobalValue"] = valueOut + "_updated";

        context.TraceStop("End executing {0}", context.JobDisplayName);
    }
}
}
