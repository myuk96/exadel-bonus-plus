﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExadelBonusPlus.Services.Properties {
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
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ExadelBonusPlus.Services.Properties.Resources", typeof(Resources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Create error.
        /// </summary>
        internal static string CreateError {
            get {
                return ResourceManager.GetString("CreateError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete model error.
        /// </summary>
        public static string DeleteError {
            get {
                return ResourceManager.GetString("DeleteError", resourceCulture);
            }
        }
        /// <summary>
        /// Looks up a localized string similar to Validation model error
        /// </summary>
        internal static string ValidationError
        {
            get
            {
                return ResourceManager.GetString("ValidationError", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Model does not find by id.
        /// </summary>
        public static string FindbyIdError {
            get {
                return ResourceManager.GetString("FindbyIdError", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Model(s) does not find.
        /// </summary>
        public static string FindError {
            get {
                return ResourceManager.GetString("FindError", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to An attempt to get with a null identifier.
        /// </summary>
        public static string IdentifierIsNull {
            get {
                return ResourceManager.GetString("IdentifierIsNull", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Login failed.
        /// </summary>
        public static string LoginFailed {
            get {
                return ResourceManager.GetString("LoginFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Model can&apos;t be null.
        /// </summary>
        public static string ModelIsNull {
            get {
                return ResourceManager.GetString("ModelIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property of sorting does not exist.
        /// </summary>
        public static string PropertyDoesNotExist
        {
            get { return ResourceManager.GetString("PropertyDoesNotExist", resourceCulture); }
        }

        public static string UserInRole
        {
            get
            {
                return ResourceManager.GetString("Current user in chosen role", resourceCulture);
            }
        }

        public static string CreateError
        {
            get
            {
                return ResourceManager.GetString("Create error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Company not found by ID.
        /// </summary>
        public static string VendorFindbyIdError {
            get {
                return ResourceManager.GetString("VendorFindbyIdError", resourceCulture);
            }
        }
    }
}
