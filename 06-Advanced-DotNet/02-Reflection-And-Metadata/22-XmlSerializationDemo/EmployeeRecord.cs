namespace XmlSerializationDemo;

/// <summary>
/// Доменна модель облікової картки співробітника для демонстрації рефлексивної серіалізації в XML.
/// </summary>
[XmlRootTag("EmployeeRecord")]
public class EmployeeRecord
{
    [XmlAttr("id")]
    public int EmployeeId { get; set; }

    [XmlAttr("department")]
    public string Department { get; set; } = string.Empty;

    [XmlTag("FullName")]
    public string Name { get; set; } = string.Empty;

    [XmlTag("PositionTitle")]
    public string Position { get; set; } = string.Empty;

    [XmlTag("MonthlySalaryUsd")]
    public double Salary { get; set; }

    [XmlTag("IsRemoteWorker")]
    public bool IsRemote { get; set; }

    [XmlExclude]
    public string BankCardPin { get; set; } = string.Empty;
}
