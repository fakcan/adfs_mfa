﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MFA {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MFA.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;div id=&quot;loginArea&quot;&gt;
        /// &lt;form method=&quot;post&quot; id=&quot;loginForm&quot; &gt;
        ///	 &lt;input id=&quot;authMethod&quot; type=&quot;hidden&quot; name=&quot;AuthMethod&quot; value=&quot;%AuthMethod%&quot;/&gt;
        ///	 &lt;input id=&quot;context&quot; type=&quot;hidden&quot; name=&quot;Context&quot; value=&quot;%Context%&quot;/&gt;
        ///	 &lt;input id=&quot;userId&quot; type=&quot;hidden&quot; name=&quot;userId&quot; value=&quot;[USERID]&quot;/&gt;
        ///	 &lt;br&gt;
        ///	 &lt;p id=&quot;pageIntroductionText&quot;&gt;MFA doğrulama bu kullanıcı için aktif değil, devam etmek için ileriyi tıklayın&lt;/p&gt;
        ///	 &lt;div id=&quot;submissionArea&quot; class=&quot;submitMargin&quot;&gt;
        ///		&lt;input id=&quot;submitButton&quot; type=&quot;submit&quot; name=&quot;Submit&quot; value=&quot;İl [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string NoSMSLogin {
            get {
                return ResourceManager.GetString("NoSMSLogin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;div id=&quot;loginArea&quot;&gt;
        /// &lt;form method=&quot;post&quot; id=&quot;loginForm&quot; &gt;
        ///	&lt;input id=&quot;authMethod&quot; type=&quot;hidden&quot; name=&quot;AuthMethod&quot; value=&quot;%AuthMethod%&quot;/&gt;
        ///	&lt;input id=&quot;context&quot; type=&quot;hidden&quot; name=&quot;Context&quot; value=&quot;%Context%&quot;/&gt;
        ///	 &lt;input id=&quot;userId&quot; type=&quot;hidden&quot; name=&quot;userId&quot; value=&quot;[USERID]&quot;/&gt;
        ///	 &lt;br&gt;
        ///	 &lt;p id=&quot;pageIntroductionText&quot;&gt;Lütfen telefonunuza gelen Sms şifresini giriniz:&lt;/p&gt;
        ///	 &lt;label for=&quot;Input&quot; class=&quot;block&quot;&gt;SMS şifresi&lt;/label&gt;
        ///	 &lt;input id=&quot;Input&quot; name=&quot;SMS&quot; type=&quot;password&quot; value=&quot;&quot; class=&quot;text&quot; /&gt;
        ///	 &lt;div id=&quot;submiss [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SmsLogin {
            get {
                return ResourceManager.GetString("SmsLogin", resourceCulture);
            }
        }
    }
}
