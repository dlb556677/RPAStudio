﻿using System;
using System.Activities;
using System.ComponentModel;
using Plugins.Shared.Library;
using Plugins.Shared.Library.UiAutomation;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Threading;
using FlaUI.Core.AutomationElements;

namespace RPA.UIAutomation.Activities.Control
{
    [Designer(typeof(SelectMultiItemsDesigner))]
    public sealed class SelectMultiItems : CodeActivity
    {
        public new string DisplayName;
        [Browsable(false)]
        public string _DisplayName { get { return "Select Multiltems"; } }

        [Localize.LocalizedCategory("Category1")] //公共 //Public //一般公開
        [Localize.LocalizedDisplayName("DisplayName1")] //错误执行 //Error execution //エラー実行
        [Localize.LocalizedDescription("Description1")] //指定即使活动引发错误，自动化是否仍应继续 //Specifies whether automation should continue even if the activity raises an error //アクティビティでエラーが発生した場合でも自動化を続行するかどうかを指定します
        public InArgument<bool> ContinueOnError { get; set; }

        [Localize.LocalizedCategory("Category1")] //公共 //Public //一般公開
        [Localize.LocalizedDisplayName("DisplayName17")] //延迟时间(结束) //Delay time (end) //遅延時間（終了）
        [Localize.LocalizedDescription("Description30")] //执行活动后的延迟时间(以毫秒为单位),默认时间为300毫秒 //The delay in milliseconds after the activity is executed. The default time is 300 milliseconds. //アクティビティが実行された後のミリ秒単位の遅延デフォルトの時間は300ミリ秒です。
        public InArgument<Int32> DelayAfter { get; set; }

        [Localize.LocalizedCategory("Category1")] //公共 //Public //一般公開
        [Localize.LocalizedDisplayName("DisplayName18")] //延迟时间(开始) //Delay time (start) //遅延時間（開始）
        [Localize.LocalizedDescription("Description34")] //活动开始执行任何操作之前的延迟时间(以毫秒为单位),默认的时间量是300毫秒。 //The delay (in milliseconds) before the activity begins any operation, the default amount of time is 300 milliseconds. //アクティビティが操作を開始するまでの遅延（ミリ秒）。デフォルトの時間は300ミリ秒です。
        public InArgument<Int32> DelayBefore { get; set; }


        [Localize.LocalizedCategory("Category2")] //UI对象 //UI Object //UIオブジェクト
        [OverloadGroup("G1")]
        [RequiredArgument]
        [Browsable(true)]
        [Localize.LocalizedDisplayName("DisplayName2")] //窗口指示器 //Window selector //セレクター
        [Localize.LocalizedDescription("Description2")] //用于在执行活动时查找特定UI元素的Text属性 //The Text property used to find specific UI elements when performing activities //アクティビティの実行時に特定のUI要素を見つけるために使用されるTextプロパティ
        public InArgument<string> Selector { get; set; }

        [Localize.LocalizedCategory("Category2")] //UI对象 //UI Object //UIオブジェクト
        [OverloadGroup("G2")]
        [Browsable(true)]
        [Localize.LocalizedDisplayName("DisplayName3")] //UI元素 //UI Element //UI要素
        [Localize.LocalizedDescription("Description3")] //输入UIElement //Enter UIElement //UIElementを入力
        public InArgument<UiElement> Element { get; set; }

        [Localize.LocalizedCategory("Category3")] //输入 //Enter //入力
        [Browsable(true)]
        [Localize.LocalizedDisplayName("DisplayName32")] //添加到现有选择 //Add to existing selection //既存の選択に追加
        public bool AddToSelection { get; set; }

        [Localize.LocalizedCategory("Category3")] //输入 //Enter //入力
        [Browsable(true)]
        [Localize.LocalizedDisplayName("DisplayName33")] //项目组 //Project Team //プロジェクトチーム
        public InArgument<string[]> MultipleItems { get; set; }

        [Browsable(false)]
        public string SourceImgPath{ get;set;}

        [Browsable(false)]
        public string icoPath
        {
            get
            {
                return @"pack://application:,,,/RPA.UIAutomation.Activities;Component/Resources/Control/SelectMultiItems.png";
            }
        }

        private System.Windows.Visibility visi = System.Windows.Visibility.Hidden;
        [Browsable(false)]
        public System.Windows.Visibility visibility
        {
            get
            {
                return visi;
            }
            set
            {
                visi = value;
            }
        }

        public static T GetValueOrDefault<T>(ActivityContext context, InArgument<T> source, T defaultValue)
        {
            T result = defaultValue;
            if (source != null && source.Expression != null)
            {
                result = source.Get(context);
            }
            return result;
        }

        protected override void Execute(CodeActivityContext context)
        {
            string[] items = MultipleItems.Get(context);
            Int32 _delayAfter = GetValueOrDefault(context, this.DelayAfter, 300);
            Int32 _delayBefore = GetValueOrDefault(context, this.DelayBefore, 300);
            try
            {
                Thread.Sleep(_delayBefore);
                var selStr = Selector.Get(context);
                UiElement element = GetValueOrDefault(context, this.Element, null);
                if (element == null && selStr != null)
                {
                    element = UiElement.FromSelector(selStr);
                }
                if(element != null)
                {
                    if (element.IsNativeObjectAutomationElement)
                    {
                        var nativieObject = element.NativeObject as AutomationElement;
                        var comboBox = nativieObject.AsComboBox();
                        for (int i = 0; i < items.Length; i++)
                        {
                            var comboItem = comboBox.Select(items[i]);
                            if (comboItem.Text != items[i])
                            {
                                var lbox = nativieObject.AsListBox();
                                for (int j = 0; j < items.Length; j++)
                                {
                                    var listItem = lbox.Select(items[j]);
                                    if (listItem.Text != items[j])
                                    {
                                        SharedObject.Instance.Output(SharedObject.enOutputType.Error, "SelectMutiItems失败,请检查UI元素!");
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        UIAutomationCommon.HandleContinueOnError(context, ContinueOnError, "非Window元素");
                    }
                }
                else
                {
                    UIAutomationCommon.HandleContinueOnError(context, ContinueOnError, Localize.LocalizedResources.GetString("msgNoElementFound"));
                }
                Thread.Sleep(_delayAfter);
            }
            catch (Exception e)
            {
                SharedObject.Instance.Output(SharedObject.enOutputType.Error, Localize.LocalizedResources.GetString("msgErrorOccurred"), e.Message);
                if (ContinueOnError.Get(context))
                {
                }
                else
                {
                    throw;
                }
            }
        }
        private void onComplete(NativeActivityContext context, ActivityInstance completedInstance)
        {
            Console.WriteLine("ClickActivity onComplete");
        }
    }
}
