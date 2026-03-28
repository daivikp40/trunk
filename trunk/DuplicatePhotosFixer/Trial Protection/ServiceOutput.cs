using System;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlRoot(ElementName = "ckes_info")]
public class Ckes_info
{
    [XmlElement(ElementName = "kat")]
    public int Kat { get; set; }
    [XmlElement(ElementName = "ifea")]
    public int Ifea { get; set; }
    [XmlElement(ElementName = "sfn")]
    public string Sfn { get; set; }
    [XmlElement(ElementName = "url")]
    public string Url { get; set; }
    [XmlElement(ElementName = "alternate_url")]
    public string Alternate_url { get; set; }
    [XmlElement(ElementName = "cmd_args")]
    public string Cmd_args { get; set; }
    [XmlElement(ElementName = "msg")]
    public string Msg { get; set; }
    [XmlElement(ElementName = "Reserved1")]
    public string Reserved1 { get; set; }
    [XmlElement(ElementName = "Reserved2")]
    public string Reserved2 { get; set; }
    [XmlElement(ElementName = "Reserved3")]
    public string Reserved3 { get; set; }
    [XmlElement(ElementName = "Reserved4")]
    public string Reserved4 { get; set; }
    [XmlElement(ElementName = "Reserved5")]
    public string Reserved5 { get; set; }
    [XmlElement(ElementName = "fqd")]
    public string Fqd { get; set; }
    [XmlElement(ElementName = "nk")]
    public string Nk { get; set; }
    [XmlElement(ElementName = "nked")]
    public string Nked { get; set; }
    [XmlElement(ElementName = "svd")]
    public long Svd { get; set; }
    [XmlElement(ElementName = "kti")]
    public int Kti { get; set; }
    [XmlElement(ElementName = "rid")]
    public int Rid { get; set; }
    [XmlElement(ElementName = "msgid")]
    public int Msgid { get; set; }
}

[XmlRoot(ElementName = "kes_info")]
public class Kes_info /*: Ckes_info*/
{
    [XmlElement(ElementName = "kat")]
    public int Kat { get; set; }
    [XmlElement(ElementName = "ifea")]
    public int Ifea { get; set; }
    [XmlElement(ElementName = "sfn")]
    public string Sfn { get; set; }
    [XmlElement(ElementName = "url")]
    public string Url { get; set; }
    [XmlElement(ElementName = "alternate_url")]
    public string Alternate_url { get; set; }
    [XmlElement(ElementName = "cmd_args")]
    public string Cmd_args { get; set; }
    [XmlElement(ElementName = "msg")]
    public string Msg { get; set; }
    [XmlElement(ElementName = "Reserved1")]
    public string Reserved1 { get; set; }
    [XmlElement(ElementName = "Reserved2")]
    public string Reserved2 { get; set; }
    [XmlElement(ElementName = "Reserved3")]
    public string Reserved3 { get; set; }
    [XmlElement(ElementName = "Reserved4")]
    public string Reserved4 { get; set; }
    [XmlElement(ElementName = "Reserved5")]
    public string Reserved5 { get; set; }
    [XmlElement(ElementName = "fqd")]
    public string Fqd { get; set; }
    [XmlElement(ElementName = "nk")]
    public string Nk { get; set; }
    [XmlElement(ElementName = "nked")]
    public string Nked { get; set; }
    [XmlElement(ElementName = "svd")]
    public long Svd { get; set; }
    [XmlElement(ElementName = "kti")]
    public int Kti { get; set; }
    [XmlElement(ElementName = "rid")]
    public int Rid { get; set; }
    [XmlElement(ElementName = "msgid")]
    public int Msgid { get; set; }
    [XmlElement(ElementName = "ks")]
    public string Ks { get; set; }
    [XmlElement(ElementName = "nDyI")]
    public string NDyI { get; set; }
    [XmlElement(ElementName = "vd")]
    public string Vd { get; set; }
}

[XmlRoot(ElementName = "ServiceOutput")]
public class ServiceOutput
{
    [XmlElement(ElementName = "sgs")]
    public string Sgs { get; set; }
    [XmlElement(ElementName = "ks")]
    public string Ks { get; set; }
    [XmlElement(ElementName = "update_ini")]
    public string Update_ini { get; set; }
    [XmlElement(ElementName = "ckes_info")]
    public Ckes_info Ckes_info { get; set; }
    [XmlElement(ElementName = "kes_info")]
    public Kes_info Kes_info { get; set; }
    [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Xsd { get; set; }
    [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Xsi { get; set; }
}


