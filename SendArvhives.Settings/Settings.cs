namespace SendArvhives.Settings
{
    /// <summary>
    /// Class for storing settings
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Language code. </summary>
        /// <value>
        /// The property is used to connect the appropriate language dictionary.
        /// </value>
        public string KeyLanguage { get; set; }
        /// <summary>
        /// Displaying the Help tab. </summary>
        /// <value>
        /// The property allows you to enable the display of the tab Help.
        /// </value>
        public bool CanShowTabHelp { get; set; }
        /// <summary>
        /// Change the subject line of the following letters. </summary>
        /// <value>
        /// The property allows you to change the text of the subject of the next emails.
        /// </value>
        public bool CanChangeTextSubjectNextEmail { get; set; }
        /// <summary>
        /// Use signature in emails. </summary>
        /// <value>
        /// The property allows you to use the signature in emails.
        /// </value>
        public bool CanUseSignature { get; set; }
        /// <summary>
        /// Use signature in emails only first email. </summary>
        /// <value>
        /// The property allows you to use the signature in emails only first email.
        /// </value>
        public bool CanUseSignatureOnlyFirstEmail { get; set; }
        /// <summary>
        /// Path file signature emails. </summary>
        /// <value>
        /// The property signature path file emails.
        /// </value>
        public string SignaturePath { get; set; }
        /// <summary>
        /// Use integral archiver. </summary>
        /// <value>
        /// The property allows use integral archiver.
        /// </value>
        public bool CanUseIntegralArchiver { get; set; }
        /// <summary>
        /// Name archive default. </summary>
        /// <value>
        /// The property name archive default.
        /// </value>
        public string NameArchiveDefault { get; set; }
        /// <summary>
        /// Size part archive (Mbyte). </summary>
        /// <value>
        /// The property size part archive (Mbyte).
        /// </value>
        public byte SizePartArchive { get; set; }
        /// <summary>
        /// Folder temp archive. </summary>
        /// <value>
        /// The property path folder temp archive.
        /// </value>
        public string ArchiveTempFolder { get; set; }
        /// <summary>
        /// Time of delay sending (sec). </summary>
        /// <value>
        /// The property time of delay sending email (sec).
        /// </value>
        public int TimeDelaySending { get; set; }
        /// <summary>
        /// Use request a delivery report. </summary>
        /// <value>
        /// The property allows use request a delivery report.
        /// </value>
        public bool CanRequestDeliveryReport { get; set; }
        /// <summary>
        /// Use request a delivery report only first email. </summary>
        /// <value>
        /// The property allows use request a delivery report only first email.
        /// </value>
        public bool CanRequestDeliveryReportOnlyFirstEmail { get; set; }
        /// <summary>
        /// Use request a read receipt. </summary>
        /// <value>
        /// The property allows use request a read receipt.
        /// </value>
        public bool CanRequestReadReport { get; set; }
        /// <summary>
        /// Use request a read receipt only first email. </summary>
        /// <value>
        /// The property allows use request a read receipt only first email.
        /// </value>
        public bool CanRequestReadReportOnlyFirstEmail { get; set; }
    }
}