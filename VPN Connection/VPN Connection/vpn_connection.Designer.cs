﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VPN_Connection {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class vpn_connection : global::System.Configuration.ApplicationSettingsBase {
        
        private static vpn_connection defaultInstance = ((vpn_connection)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new vpn_connection())));
        
        public static vpn_connection Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("www.petrolcard.hu")]
        public string vpn_host {
            get {
                return ((string)(this["vpn_host"]));
            }
            set {
                this["vpn_host"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("jundjkft")]
        public string vpn_password {
            get {
                return ((string)(this["vpn_password"]));
            }
            set {
                this["vpn_password"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("jundjkft")]
        public string vpn_username {
            get {
                return ((string)(this["vpn_username"]));
            }
            set {
                this["vpn_username"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("petrolcard.hu")]
        public string vpn_entry_name {
            get {
                return ((string)(this["vpn_entry_name"]));
            }
            set {
                this["vpn_entry_name"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool notification {
            get {
                return ((bool)(this["notification"]));
            }
            set {
                this["notification"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int notification_length {
            get {
                return ((int)(this["notification_length"]));
            }
            set {
                this["notification_length"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int max_attempt_to_reconnect {
            get {
                return ((int)(this["max_attempt_to_reconnect"]));
            }
            set {
                this["max_attempt_to_reconnect"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int checking_state_interval {
            get {
                return ((int)(this["checking_state_interval"]));
            }
            set {
                this["checking_state_interval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("21:00:00")]
        public global::System.TimeSpan silent_mode_start {
            get {
                return ((global::System.TimeSpan)(this["silent_mode_start"]));
            }
            set {
                this["silent_mode_start"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("07:00:00")]
        public global::System.TimeSpan silent_mode_end {
            get {
                return ((global::System.TimeSpan)(this["silent_mode_end"]));
            }
            set {
                this["silent_mode_end"] = value;
            }
        }
    }
}
