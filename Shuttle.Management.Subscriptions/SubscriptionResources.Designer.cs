﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shuttle.Management.Subscriptions {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SubscriptionResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SubscriptionResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Shuttle.Management.Subscriptions.SubscriptionResources", typeof(SubscriptionResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ImageSubscriptions {
            get {
                object obj = ResourceManager.GetObject("ImageSubscriptions", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Accept.
        /// </summary>
        internal static string TextAccept {
            get {
                return ResourceManager.GetString("TextAccept", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Decline.
        /// </summary>
        internal static string TextDecline {
            get {
                return ResourceManager.GetString("TextDecline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Get assembly types.
        /// </summary>
        internal static string TextGetAssemblyTypes {
            get {
                return ResourceManager.GetString("TextGetAssemblyTypes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Refresh subscribers.
        /// </summary>
        internal static string TextRefreshSubscribers {
            get {
                return ResourceManager.GetString("TextRefreshSubscribers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Refresh subscriptions.
        /// </summary>
        internal static string TextRefreshSubscriptions {
            get {
                return ResourceManager.GetString("TextRefreshSubscriptions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Subscriptions.
        /// </summary>
        internal static string TextSubscriptions {
            get {
                return ResourceManager.GetString("TextSubscriptions", resourceCulture);
            }
        }
    }
}
