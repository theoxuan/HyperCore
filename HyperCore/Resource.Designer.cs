﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18063
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HyperCore {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HyperCore.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
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
        ///   查找类似 CREATE TABLE IF NOT EXISTS &apos;Card&apos;(&apos;id&apos; TEXT NOT NULL,&apos;zid&apos; TEXT,&apos;var&apos; TEXT,&apos;name&apos; TEXT NOT NULL,&apos;zname&apos; TEXT,&apos;set&apos; TEXT NOT NULL,&apos;setcode&apos; TEXT NOT NULL,&apos;color&apos; TEXT,&apos;colorcode&apos; TEXT,&apos;cost&apos; TEXT,&apos;cmc&apos; TEXT,&apos;type&apos; TEXT NOT NULL,&apos;ztype&apos; TEXT,&apos;typecode&apos; TEXT NOT NULL,&apos;pow&apos; TEXT,&apos;tgh&apos; TEXT,&apos;loyalty&apos; TEXT,&apos;text&apos; TEXT,&apos;ztext&apos; TEXT,&apos;flavor&apos; TEXT,&apos;zflavor&apos; TEXT,&apos;artist&apos; TEXT,&apos;rarity&apos; TEXT NOT NULL,&apos;raritycode&apos; TEXT NOT NULL,&apos;number&apos; TEXT NOT NULL,&apos;rulings&apos; TEXT,&apos;legality&apos; TEXT,&apos;rating&apos; TEXT,PRIMARY KEY(&apos;id&apos;));
        ///CRE [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string BuildCommand {
            get {
                return ResourceManager.GetString("BuildCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Provider=Microsoft.Jet.OLEDB.4.0;Data Source=DATA.ac 的本地化字符串。
        /// </summary>
        internal static string ConnectionCommandAccess {
            get {
                return ResourceManager.GetString("ConnectionCommandAccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 data source=DATA.db;password=5AEB55D5-F169-4EB2-A768-B20EBD20151E 的本地化字符串。
        /// </summary>
        internal static string ConnectionCommandSQLite {
            get {
                return ResourceManager.GetString("ConnectionCommandSQLite", resourceCulture);
            }
        }
    }
}