using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Reflection;
using FRS.Lib.Model;

namespace FRS.Lib.Service.REQ
{
    public class POFormXmlService
    {
        public POFormXmlService(string formVersionId, UrgentLevel Urgentlevel, string account, string userGuid, string userName, SW_PO_HEAD head, List<SW_PO_GRID> grid)
        {
            this.FormVersionId = formVersionId;
            this.Urgentlevel = Urgentlevel;
            this.Account = account;
            this.UserGuid = userGuid;
            this.UserName = userName;
            Fields = new FieldList();
            ApplyAttach = new List<string>();
            this.head = head;
            this.grid = grid;
        }

        SW_PO_HEAD head;
        List<SW_PO_GRID> grid;
        List<string> gridList = new List<string>() { "Name", "Spec", "Quantity", "UnitPrice", "Amount" };
        #region==================申請資訊==========================
        public string FormVersionId { get; set; } = "";
        public UrgentLevel Urgentlevel { get; set; } = UrgentLevel.Urgent;
        public string Account { get; set; } = "";
        public string UserGuid { get; set; } = "";
        public string UserName { get; set; } = "";
        public string GroupId { get; set; } = "";
        public string JobTitleId { get; set; } = "";
        public string Comment { get; set; } = "";
        public bool IsNeedTransfer { get; set; } = true;
        public bool IsDeleteTemp { get; set; } = true;

        public List<string> ApplyAttach { get; set; } = new List<string>();
        public FieldList Fields { get; set; } = new FieldList();

        public MessageContent MessageContent { get; set; } = new MessageContent();
        public DisplayTitle DisplayTitle { get; set; } = new DisplayTitle();
        #endregion

        public string ConvertToFormInfoXml()
        {
            //表單表頭資訊
            XElement formXe = new XElement("Form", new XAttribute("formVersionId", this.FormVersionId)
                , new XAttribute("urgentLevel", (int)this.Urgentlevel));

            //申請者區塊
            XElement applicantXe = new XElement("Applicant", new XAttribute("account", this.Account)
                , new XAttribute("groupId", this.GroupId), new XAttribute("jobTitleId", this.JobTitleId));
            //申請者意見
            XElement CommentXe = new XElement("Comment", this.Comment);

            //申請附件
            if (ApplyAttach.Count > 0)
            {
                XElement attachXe = new XElement("Attach", new XAttribute("IsNeedTransfer", this.IsNeedTransfer)
              , new XAttribute("IsDeleteTemp", this.IsDeleteTemp));

                applicantXe.Add(attachXe);

                foreach (var path in ApplyAttach)
                {
                    XElement attachItemXe = new XElement("AttachItem",
                        new XAttribute("filePath", path));

                    attachXe.Add(attachItemXe);
                }
            }

            formXe.Add(applicantXe);
            applicantXe.Add(CommentXe);


            //欄位
            XElement FormFieldValueXE = new XElement("FormFieldValue");
            formXe.Add(FormFieldValueXE);
            foreach (var Propertie in this.Fields.GetType().GetProperties())
            {

                var field = ConvertToChildObject(Propertie, this.Fields);
                XElement FieldItemXE = new XElement("FieldItem", new XAttribute("fieldId", field.FieldId));
                FormFieldValueXE.Add(FieldItemXE);

                //排除掉用不到fieldValue的欄位
                switch (field.FieldType)
                {
                    case FieldType.optionalField:
                        FileField optionalField = (FileField)field;
                        break;
                    case FieldType.dataGrid:
                        XElement DataGridElement = new XElement("DataGrid");
                        FieldItemXE.Add(DataGridElement);
                        if (grid.Count > 0)
                        {
                            int rowIndex = 0;
                            foreach (var row in grid)
                            {
                                XElement rowElement = new XElement("Row", new XAttribute("order", rowIndex.ToString()));
                                rowIndex++;
                                foreach (var row2 in gridList)
                                {
                                    XElement cellElement = new XElement("Cell", new XAttribute("fieldId", row2));
                                    switch (row2)
                                    {
                                        case "Name":
                                            cellElement.Add(new XAttribute("fieldValue", row.Name));
                                            break;
                                        case "Spec":
                                            cellElement.Add(new XAttribute("fieldValue", row.Spec));
                                            break;
                                        case "Quantity":
                                            cellElement.Add(new XAttribute("fieldValue", row.Quantity));
                                            break;
                                        case "UnitPrice":
                                            cellElement.Add(new XAttribute("fieldValue", row.UnitPrice));
                                            break;
                                        case "Amount":
                                            cellElement.Add(new XAttribute("fieldValue", row.Amount));
                                            break;
                                        default:
                                            cellElement.Add(new XAttribute("fieldValue", ""));
                                            break;
                                    }
                                    rowElement.Add(cellElement);
                                }

                                DataGridElement.Add(rowElement);
                            }

                            FieldItemXE.Add(new XAttribute("fillerName", this.UserName));
                            FieldItemXE.Add(new XAttribute("fillerUserGuid", this.UserGuid));
                            FieldItemXE.Add(new XAttribute("fillerAccount", this.Account));
                            FieldItemXE.Add(new XAttribute("fillSiteId", ""));
                        }
                        break;
                    case FieldType.fileButton:
                        FileField fileField = (FileField)field;

                        if (fileField.FileAttach.Count > 0)
                        {
                            FieldItemXE.Add(new XAttribute("IsNeedTransfer", fileField.IsNeedTransfer.ToString()));
                            FieldItemXE.Add(new XAttribute("IsDeleteTemp", fileField.IsDeleteTemp.ToString()));
                            FieldItemXE.Add(new XAttribute("fillerName", this.UserName));
                            FieldItemXE.Add(new XAttribute("fillerUserGuid", this.UserGuid));
                            FieldItemXE.Add(new XAttribute("fillerAccount", this.Account));
                            FieldItemXE.Add(new XAttribute("fillSiteId", ""));

                            foreach (var filePath in fileField.FileAttach)
                            {
                                XElement attachItemXE = new XElement("AttachItem", new XAttribute("filePath", filePath));
                                FieldItemXE.Add(attachItemXE);
                            }

                        }

                        break;
                    default:
                        switch (field.FieldId)
                        {
                            case "formNo":
                                FieldItemXE.Add(new XAttribute("fieldValue", ""));
                                break;
                            case "Date":
                                FieldItemXE.Add(new XAttribute("fieldValue", head.Date));
                                break;
                            case "Dept":
                                FieldItemXE.Add(new XAttribute("fieldValue", head.Dept[1]));
                                FieldItemXE.Add(new XAttribute("realValue", $"<UserSet><Element type='group' isDepth='False'><groupId>" + head.Dept[0] + "</groupId></Element></UserSet>\r\n"));
                                break;
                            case "Applicant":
                                FieldItemXE.Add(new XAttribute("fieldValue", head.Applicant[0] + "(" + head.Applicant[1] + ")"));
                                FieldItemXE.Add(new XAttribute("realValue", $"<UserSet><Element type='user'> <userId>" + head.Applicant[2] + "</userId></Element></UserSet>\r\n"));
                                break;
                            case "PODept":
                                FieldItemXE.Add(new XAttribute("fieldValue", head.PODept));
                                break;
                            case "POApplicant":
                                FieldItemXE.Add(new XAttribute("fieldValue", UserName + "(" + Account + ")"));
                                FieldItemXE.Add(new XAttribute("realValue", $"<UserSet><Element type='user'> <userId>" + UserGuid + "</userId></Element></UserSet>\r\n"));
                                break;
                            /*case "POSignOff":
                                if (head.POSignOff != null)
                                {
                                    FieldItemXE.Add(new XAttribute("fieldValue", head.POSignOff[0] + "(" + head.POSignOff[1] + ")"));
                                    FieldItemXE.Add(new XAttribute("realValue", $"<UserSet><Element type='user'> <userId>" + head.POSignOff[2] + "</userId></Element></UserSet>\r\n"));
                                }
                                else
                                    FieldItemXE.Add(new XAttribute("fieldValue", ""));
                                break;*/
                            case "REQForm":
                                XElement testFElement = new XElement("FormChooseInfo", new XAttribute("taskGuid", head.REQForm));
                                FieldItemXE.Add(testFElement);
                                break;
                            case "TotalPrice":
                                FieldItemXE.Add(new XAttribute("fieldValue", head.TotalPrice));
                                break;
                            default:
                                FieldItemXE.Add(new XAttribute("fieldValue", ""));
                                break;
                        }
                        break;
                }

                if (field.FieldId != "formNo" && field.FieldId != "TotalPrice" && field.FieldValue != "")
                {
                    FieldItemXE.Add(new XAttribute("fillerName", this.UserName));
                    FieldItemXE.Add(new XAttribute("fillerUserGuid", this.UserGuid));
                    FieldItemXE.Add(new XAttribute("fillerAccount", this.Account));
                    FieldItemXE.Add(new XAttribute("fillSiteId", ""));
                }

                //加殊屬性的欄位
                switch (field.FieldType)
                {
                    case FieldType.autoNumber:
                        // <FieldItem fieldId="NO" fieldValue="" IsNeedAutoNbr ="false" />   
                        FieldItemXE.Add(new XAttribute("IsNeedAutoNbr", ((AutoNumnerField)field).IsNeedAutoNbr));

                        break;
                    case FieldType.numberText:
                    case FieldType.singleLineText:

                        break;
                }

            }

            //起單顯示標題
            /*XElement jsonDisplayXE = new XElement("JsonDisplay", Newtonsoft.Json.JsonConvert.SerializeObject(DisplayTitle));
            formXe.Add(jsonDisplayXE);

            //起單郵件樣板
            XElement messageContent = new XElement("MessageContent");
            formXe.Add(messageContent);

            foreach (var Propertie in this.MessageContent.GetType().GetProperties())
            {
                XElement contentXE = new XElement(Propertie.Name.ToString().Substring(1));
              
                contentXE.Value = Propertie.GetValue(this.MessageContent).ToString();
                messageContent.Add(contentXE);
            }*/

            return formXe.ToString();
        }

        private Field ConvertToChildObject(PropertyInfo propertyInfo, object parent)
        {
            var source = propertyInfo.GetValue(parent);
            var destination = Activator.CreateInstance(propertyInfo.PropertyType);

            foreach (PropertyInfo prop in destination.GetType().GetProperties().ToList())
            {
                var value = source.GetType().GetProperty(prop.Name).GetValue(source, null);
                prop.SetValue(destination, value, null);
            }

            return (Field)destination;
        }

        private string ConvertToChildObjectForString(PropertyInfo propertyInfo, object parent)
        {
            var source = propertyInfo.GetValue(parent);
            var destination = Activator.CreateInstance(propertyInfo.PropertyType);

            foreach (PropertyInfo prop in destination.GetType().GetProperties().ToList())
            {
                var value = source.GetType().GetProperty(prop.Name).GetValue(source, null);
                prop.SetValue(destination, value, null);
            }

            return (string)destination;
        }

    }


        #region===================列舉==========================
        /// <summary>
        /// 表單的緊急程度
        /// </summary>
        public enum UrgentLevel : int
        {
            /// <summary>
            /// 緊急
            /// </summary>
            MostUrgent,
            /// <summary>
            /// 急
            /// </summary>
            Urgent,
            /// <summary>
            /// 普通
            /// </summary>
            Normal

        }//end UrgentLevel


        /// <summary>
        /// 表單版本欄位的欄位格式
        /// </summary>
        /// <remarks></remarks>
        public enum FieldType : int
        {
            /// <summary>
            /// 單行文字欄位singleLineText
            /// </summary>
            singleLineText = 0,

            /// <summary>
            /// 多行文字欄位multiLineText
            /// </summary>
            multiLineText = 1,

            /// <summary>
            /// 數值欄位numberText
            /// </summary>
            numberText = 2,

            /// <summary>
            /// 檔案選取欄位fileButton
            /// </summary>
            fileButton = 3,

            /// <summary>
            /// 日期欄位dateSelect
            /// </summary>
            dateSelect = 4,

            /// <summary>
            /// 時間欄位timeSelect
            /// </summary>
            timeSelect = 5,

            /// <summary>
            /// 核選方塊checkBox
            /// </summary>
            checkBox = 6,

            /// <summary>
            /// 單選鈕radioButton
            /// </summary>
            radioButton = 7,

            /// <summary>
            /// 下拉式選單dropDownList
            /// </summary>
            dropDownList = 8,

            /// <summary>
            /// 明細欄位dataGrid
            /// </summary>
            dataGrid = 9,

            /// <summary>
            /// 超連結hyperLink
            /// </summary>
            hyperLink = 10,

            /// <summary>
            /// 自動編號autoNumber
            /// </summary>
            autoNumber = 11,

            /// <summary>
            /// 表單計算欄位calculateText
            /// </summary>
            calculateText = 12,

            /// <summary>
            /// 申請者(特定值欄位)userProposer
            /// </summary>
            userProposer = 13,

            /// <summary>
            /// 申請者部門(特定值欄位)userDept
            /// </summary>
            userDept = 14,

            /// <summary>
            /// 申請者職級(特定值欄位)userRank
            /// </summary>
            userRank = 15,

            /// <summary>
            /// 所有部門(特定值欄位)allDept
            /// </summary>
            allDept = 16,

            /// <summary>
            /// 所有職級(特定值欄位)allRank
            /// </summary>
            allRank = 17,

            /// <summary>
            /// 所有職務(特定值欄位)allFunction
            /// </summary>
            allFunction = 18,

            /// <summary>
            /// 所有人員(特定值欄位)allUser
            /// </summary>
            allUser = 19,

            /// <summary>
            /// 加總平均欄位
            /// </summary>
            aggregateText = 20,

            /// <summary>
            /// 隱藏欄位
            /// </summary>
            hiddenField = 21,

            /// <summary>
            /// 外掛欄位
            /// </summary>
            optionalField = 22,


            /// <summary>
            /// 人員組織欄位
            /// </summary>
            /// <remarks>
            ///2011/12/29 add by cloudmikado
            /// </remarks>
            userSetField = 23,

            /// <summary>
            /// 申請者代理人
            /// </summary>
            userAgent = 24,
            /// <summary>
            /// 文字編輯欄位
            /// </summary>
            htmlEditor = 25,

            /// <summary>
            /// 純顯示欄位
            /// </summary>
            displayField = 26,

            /// <summary>
            /// 所有會員欄位
            /// </summary>
            allMember = 27,
            /// <summary>
            /// 所有群組
            /// </summary>
            allMemberGroup = 28,
            /// <summary>
            /// 手寫簽名
            /// </summary>
            canvas = 29,

            /// <summary>
            /// 申請者職務
            /// </summary>
            userFunction = 30,

            /// <summary>
            /// 申請者資訊
            /// (非實際存在的欄位，條件比對用)
            /// </summary>
            applyInformation = 31,

            none = 999
        }
        #endregion

        #region==================欄位物件==========================
        /// <summary>
        /// 表單編號欄位
        /// </summary>
        public class AutoNumnerField : Field
        {
            public AutoNumnerField()
            {
                base.FieldType = FieldType.autoNumber;
            }
            public string IsNeedAutoNbr { get; set; } = "false";
        }


        #region==================明細欄位物件==========================
        /// <summary>
        /// 明細欄位物件
        /// </summary>
        public class DataGridField : Field
        {
            public List<CellCollections> Rows { get; set; } = new List<CellCollections>();
            public DataGridField()
            {
                base.FieldType = FieldType.dataGrid;
            }

        }

        public class CellCollections
        {
        }

        #endregion

        /// <summary>
        /// 明細欄位物件
        /// </summary>
        public class OptionalField : Field
        {
            public string ConditionValue { get; set; } = "";
        }

        /// <summary>
        /// 附件欄位物件
        /// </summary>
        public class FileField : Field
        {
            public bool IsNeedTransfer { get; set; } = true;
            public bool IsDeleteTemp { get; set; } = true;

            public List<string> FileAttach { get; set; } = new List<string>();
        }

        /// <summary>
        /// 通用欄位物件
        /// </summary>
        public class Field
        {

            public string FieldId { get; set; } = "";
            public string FieldName { get; set; } = "";
            public string FieldValue { get; set; } = "";
            public FieldType FieldType { get; set; } = FieldType.singleLineText;
            public string RealValue { get; set; } = "";
            public string FillerName { get; set; } = "";
            public string FillerUserGuid { get; set; } = "";
            public string FillerAccount { get; set; } = "";
        }


        #endregion

    #region==================欄位清單==========================

        public class _DetailCellCollections : CellCollections
        {
            
            public Field _NameField { get; set; } = new Field() { FieldType = FieldType.singleLineText, FieldId="Name" };

            public Field _SpecField { get; set; } = new Field() { FieldType = FieldType.singleLineText, FieldId="Spec" };

            public Field _QuantityField { get; set; } = new Field() { FieldType = FieldType.numberText, FieldId="Quantity" };

            public Field _UnitPriceField { get; set; } = new Field() { FieldType = FieldType.numberText, FieldId="UnitPrice" };

            public Field _AmountField { get; set; } = new Field() { FieldType = FieldType.calculateText, FieldId="Amount" };
                    
        }



    public class FieldList
    {

        public AutoNumnerField _formNoField { get; set; } = new AutoNumnerField() { FieldId = "formNo" };

        public Field _DateField { get; set; } = new Field() { FieldType = FieldType.dateSelect, FieldId="Date" };

        public Field _DeptField { get; set; } = new Field() { FieldType = FieldType.allDept, FieldId="Dept" };

        public Field _ApplicantField { get; set; } = new Field() { FieldType = FieldType.allUser, FieldId="Applicant" };

        public Field _PODeptField { get; set; } = new Field() { FieldType = FieldType.allDept, FieldId = "PODept" };

        public Field _POApplicantField { get; set; } = new Field() { FieldType = FieldType.allUser, FieldId = "POApplicant" };

        public Field _ACCEPTApplicantField { get; set; } = new Field() { FieldType = FieldType.allUser, FieldId = "ACCEPTApplicant" };

        public Field _POSignOffField { get; set; } = new Field() { FieldType = FieldType.allUser, FieldId = "POSignOff" };

        public OptionalField _REQFormField { get; set; } = new OptionalField() { FieldId = "REQForm" };

        public Field _VendorField { get; set; } = new Field() { FieldType = FieldType.singleLineText, FieldId="Vendor" };

        public Field _POSignField { get; set; } = new Field() { FieldType = FieldType.radioButton, FieldId = "POSign" };

        public Field _POSignNoField { get; set; } = new Field() { FieldType = FieldType.singleLineText, FieldId = "POSignNo" };

        public Field _BargainSignField { get; set; } = new Field() { FieldType = FieldType.radioButton, FieldId = "BargainSign" };

        public Field _BargainSignNoField { get; set; } = new Field() { FieldType = FieldType.singleLineText, FieldId = "BargainSignNo" };

        public DataGridField _DetailField { get; set; } = new DataGridField() { FieldId = "Detail" };



        public Field _TotalPriceField { get; set; } = new Field() { FieldType = FieldType.aggregateText, FieldId="TotalPrice" };

        public Field _DirectionsField { get; set; } = new Field() { FieldType = FieldType.multiLineText, FieldId="Directions" };

        public FileField _FileField { get; set; } = new FileField() { FieldId = "File" };

    }

    public class MessageContent
    {
        
        public string _formNo { get; set; } = "";

        public string _Date { get; set; } = "";

        public string _Dept { get; set; } = "";

        public string _Applicant { get; set; } = "";

        public string _REQForm { get; set; } = "";

        public string _Vendor { get; set; } = "";

        public string _Detail { get; set; } = "";

        public string _TotalPrice { get; set; } = "";

        public string _Directions { get; set; } = "";

        public string _File { get; set; } = "";

    }

    public class DisplayTitle
    {
        
        public string _formNo { get; set; } = "";

        public string _Date { get; set; } = "";

        public string _Dept { get; set; } = "";

        public string _Applicant { get; set; } = "";

        public string _REQForm { get; set; } = "";

        public string _Vendor { get; set; } = "";

        public string _Detail { get; set; } = "";

        public string _TotalPrice { get; set; } = "";

        public string _Directions { get; set; } = "";

        public string _File { get; set; } = "";

    }
    #endregion
}