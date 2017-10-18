using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using System.Threading;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Globalization;

namespace HWID_Spoofer
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.BackColor = Color.FromArgb(29, 29, 29);
            listBox1.ForeColor = Color.FromArgb(255, 255, 255);
        }

        private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(metroCheckBox1.Checked)
            {
                metroCheckBox2.Checked = true;
                metroCheckBox3.Checked = true;
                metroCheckBox4.Checked = true;
                metroCheckBox5.Checked = true;
                metroCheckBox6.Checked = true;
                metroCheckBox7.Checked = true;
                metroCheckBox8.Checked = true;
                metroCheckBox9.Checked = true;
            }
            else
            {
                metroCheckBox2.Checked = false;
                metroCheckBox3.Checked = false;
                metroCheckBox4.Checked = false;
                metroCheckBox5.Checked = false;
                metroCheckBox6.Checked = false;
                metroCheckBox7.Checked = false;
                metroCheckBox8.Checked = false;
                metroCheckBox9.Checked = false;
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            //Form2 f2 = new Form2();
            //f2.Show();
            DialogResult result = MessageBox.Show("Are you sure you would like to continue?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                Spoof();
            }
        }

        public void Log(string log)
        {
            listBox1.Items.Add(log);
        }
        private void metroCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(metroCheckBox2.Checked)
            {
                Global.PCName = true;
            }
            else
            {
                Global.PCName = false;
            }
        }

        private void metroCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox3.Checked)
            {
                Global.GUID = true;
            }
            else
            {
                Global.GUID = false;
            }
        }

        private void metroCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox4.Checked)
            {
                Global.HWIDserial = true;
            }
            else
            {
                Global.HWIDserial = false;
            }
        }

        private void metroCheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox5.Checked)
            {
                Global.ProductID = true;
            }
            else
            {
                Global.ProductID = false;
            }
        }

        private void metroCheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox6.Checked)
            {
                Global.MacAddress = true;
            }
            else
            {
                Global.MacAddress = false;
            }
        }

        private void metroCheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox7.Checked)
            {
                Global.InstallTime = true;
            }
            else
            {
                Global.InstallTime = false;
            }
        }

        private void metroCheckBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox8.Checked)
            {
                Global.InstallDate = true;
            }
            else
            {
                Global.InstallDate = false;
            }
        }

        private void metroCheckBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox9.Checked)
            {
                Global.HwProfileGUID = true;
            }
            else
            {
                Global.HwProfileGUID = false;
            }
        }

        public void Spoof()
        {
            listBox1.Items.Clear();
            if (Global.PCName)
            {
                SpoofPCName();
            }
            if (Global.GUID)
            {
                SpoofGUID();
            }
            if (Global.HWIDserial)
            {
                SpoofHWIDserial();
            }
            if (Global.MacAddress)
            {
                SpoofMacAddress();
            }
            if (Global.ProductID)
            {
                SpoofProductID();
            }
            if (Global.InstallTime)
            {
                SpoofInstallTime();
            }
            if (Global.InstallDate)
            {
                SpoofInstallDate();
            }
            if (Global.HwProfileGUID)
            {
                SpoofHwProfileGUID();
            }
            MessageBox.Show("Please restart your computer.", "Restart");
        }

        #region MACAddressSpoof

        Random rand = new Random();
        public const string Alphabet = "ABCDEF0123456789";
        public string GenerateString(int size)
        {
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }
            return new string(chars);
        }

        void SpoofMacAddress()
        {
            Log("Current MAC Address: " + CurrentMAC());

            string newMACAddress = "00" + GenerateString(10);
            RegistryKey mac = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\0012", true);
            mac.SetValue("NetworkAddress", newMACAddress);
            mac.Close();
            Log("MAC address changed to: " + CurrentMAC());
            Log("");
        }

        string CurrentMAC()
        {
            RegistryKey mac;
            string MAC;
            mac = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\0012", true);
            MAC = (string)mac.GetValue("NetworkAddress");
            mac.Close();
            return MAC;
        }
        #endregion

        #region GUIDspoof
        void SpoofGUID()
        {
            Log("Current GUID: " + CurrentGUID());

            string newGUID = Guid.NewGuid().ToString();

            RegistryKey OurKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            OurKey = OurKey.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", true);
            OurKey.SetValue("MachineGuid", newGUID);

            Log("GUID changed to: " + CurrentGUID());
            Log("");
        }

        public static string CurrentGUID()
        {
            string location = @"SOFTWARE\Microsoft\Cryptography";
            string name = "MachineGuid";

            using (RegistryKey localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey rk = localMachineX64View.OpenSubKey(location))
                {
                    if (rk == null)
                        throw new KeyNotFoundException(string.Format("Key Not Found: {0}", location));

                    object machineGuid = rk.GetValue(name);
                    if (machineGuid == null)
                        throw new IndexOutOfRangeException(string.Format("Index Not Found: {0}", name));

                    return machineGuid.ToString();
                }
            }
        }
        #endregion

        #region ProductIDspoof
        void SpoofProductID()
        {
            Log("Current ProductID: " + CurrentProductID());
            string newProductID = GenerateString(5) + "-" + GenerateString(5) + "-" + GenerateString(5) + "-" + GenerateString(5);

            RegistryKey OurKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            OurKey = OurKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", true);
            OurKey.SetValue("ProductID", newProductID);
            OurKey.Close();

            Log("ProductID changed to: " + CurrentProductID());
            Log("");
        }

        public static string CurrentProductID()
        {
            string location = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            string name = "ProductID";

            using (RegistryKey localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey rk = localMachineX64View.OpenSubKey(location))
                {
                    if (rk == null)
                        throw new KeyNotFoundException(string.Format("Key Not Found: {0}", location));

                    object productID = rk.GetValue(name);
                    if (productID == null)
                        throw new IndexOutOfRangeException(string.Format("Index Not Found: {0}", name));

                    return productID.ToString();
                }
            }
        }
        #endregion

        #region InstallTimeSpoof
        Random random = new Random();
        public const string Alphabet1 = "abcdef0123456789";
        public string GenerateDate(int size)
        {
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = Alphabet1[random.Next(Alphabet1.Length)];
            }
            return new string(chars);
        }

        void SpoofInstallTime()
        {
            Log("Current install time: " + CurrentInstallTime());

            string newInstallTime = GenerateDate(15);

            RegistryKey OurKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            OurKey = OurKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", true);
            OurKey.SetValue("InstallTime", newInstallTime);
            OurKey.Close();
            Log("Install time changed to: " + CurrentInstallTime());
            Log("");
        }

        public static string CurrentInstallTime()
        {
            string location = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            string name = "InstallTime";

            using (RegistryKey localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey rk = localMachineX64View.OpenSubKey(location))
                {
                    if (rk == null)
                        throw new KeyNotFoundException(string.Format("Key Not Found: {0}", location));

                    object installtime = rk.GetValue(name);
                    if (installtime == null)
                        throw new IndexOutOfRangeException(string.Format("Index Not Found: {0}", name));

                    return installtime.ToString();
                }
            }
        }
        #endregion

        #region InstallDateSpoof
        void SpoofInstallDate()
        {
            Log("Current install date: " + CurrentInstallDate());

            string newInstallDate = GenerateDate(8);

            RegistryKey OurKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            OurKey = OurKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", true);
            OurKey.SetValue("InstallDate", newInstallDate);
            OurKey.Close();

            Log("New Install Date: " + CurrentInstallDate());
            Log("");
        }

        public static string CurrentInstallDate()
        {
            string location = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            string name = "InstallDate";

            using (RegistryKey localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey rk = localMachineX64View.OpenSubKey(location))
                {
                    if (rk == null)
                        throw new KeyNotFoundException(string.Format("Key Not Found: {0}", location));

                    object installdate = rk.GetValue(name);
                    if (installdate == null)
                        throw new IndexOutOfRangeException(string.Format("Index Not Found: {0}", name));

                    return installdate.ToString();
                }
            }
        }
        #endregion

        #region HwProfileGUIDspoof
        void SpoofHwProfileGUID()
        {
            Log("Current HwProfileGUID: " + CurrentHwProfileGUID());
            string newHwProfileGUID = "{" + Guid.NewGuid().ToString() + "}";

            RegistryKey OurKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            OurKey = OurKey.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\IDConfigDB\Hardware Profiles\0001", true);
            OurKey.SetValue("HwProfileGUID", newHwProfileGUID);
            OurKey.Close();

            Log("New HwProfileGUID: " + CurrentHwProfileGUID());
            Log("");
        }

        public static string CurrentHwProfileGUID()
        {
            string location = @"SYSTEM\CurrentControlSet\Control\IDConfigDB\Hardware Profiles\0001";
            string name = "HwProfileGUID";

            using (RegistryKey localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey rk = localMachineX64View.OpenSubKey(location))
                {
                    if (rk == null)
                        throw new KeyNotFoundException(string.Format("Key Not Found: {0}", location));

                    object hwprofileguid = rk.GetValue(name);
                    if (hwprofileguid == null)
                        throw new IndexOutOfRangeException(string.Format("Index Not Found: {0}", name));

                    return hwprofileguid.ToString();
                }
            }
        }
        #endregion

        #region HddSerialSpoof
        void ChangeSerialNumber(char volume, uint newSerial)
        {
            var fsInfo = new[]
            {
        new { Name = "FAT32", NameOffs = 0x52, SerialOffs = 0x43 },
        new { Name = "FAT", NameOffs = 0x36, SerialOffs = 0x27 },
        new { Name = "NTFS", NameOffs = 0x03, SerialOffs = 0x48 }
    };

            using (var disk = new Disk(volume))
            {
                var sector = new byte[512];
                disk.ReadSector(0, sector);

                var fs = fsInfo.FirstOrDefault(
                        f => Strncmp(f.Name, sector, f.NameOffs)
                    );
                if (fs == null) throw new NotSupportedException("This file system is not supported");

                var s = newSerial;
                for (int i = 0; i < 4; ++i, s >>= 8) sector[fs.SerialOffs + i] = (byte)(s & 0xFF);

                disk.WriteSector(0, sector);
            }
        }

        bool Strncmp(string str, byte[] data, int offset)
        {
            for (int i = 0; i < str.Length; ++i)
            {
                if (data[i + offset] != (byte)str[i]) return false;
            }
            return true;
        }

        class Disk : IDisposable
        {
            private SafeFileHandle handle;

            public Disk(char volume)
            {
                var ptr = CreateFile(
                    String.Format("\\\\.\\{0}:", volume),
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite,
                    IntPtr.Zero,
                    FileMode.Open,
                    0,
                    IntPtr.Zero
                    );

                handle = new SafeFileHandle(ptr, true);

                if (handle.IsInvalid) Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            public void ReadSector(uint sector, byte[] buffer)
            {
                if (buffer == null) throw new ArgumentNullException("buffer");
                if (SetFilePointer(handle, sector, IntPtr.Zero, EMoveMethod.Begin) == INVALID_SET_FILE_POINTER) Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                uint read;
                if (!ReadFile(handle, buffer, buffer.Length, out read, IntPtr.Zero)) Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                if (read != buffer.Length) throw new IOException();
            }

            public void WriteSector(uint sector, byte[] buffer)
            {
                if (buffer == null) throw new ArgumentNullException("buffer");
                if (SetFilePointer(handle, sector, IntPtr.Zero, EMoveMethod.Begin) == INVALID_SET_FILE_POINTER) Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                uint written;
                if (!WriteFile(handle, buffer, buffer.Length, out written, IntPtr.Zero)) Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                if (written != buffer.Length) throw new IOException();
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (handle != null) handle.Dispose();
                }
            }

            enum EMoveMethod : uint
            {
                Begin = 0,
                Current = 1,
                End = 2
            }

            const uint INVALID_SET_FILE_POINTER = 0xFFFFFFFF;

            [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern IntPtr CreateFile(
                string fileName,
                [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
                [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
                IntPtr securityAttributes,
                [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
                int flags,
                IntPtr template);

            [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            static extern uint SetFilePointer(
                 [In] SafeFileHandle hFile,
                 [In] uint lDistanceToMove,
                 [In] IntPtr lpDistanceToMoveHigh,
                 [In] EMoveMethod dwMoveMethod);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool ReadFile(SafeFileHandle hFile, [Out] byte[] lpBuffer,
                int nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

            [DllImport("kernel32.dll")]
            static extern bool WriteFile(SafeFileHandle hFile, [In] byte[] lpBuffer,
                int nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten,
                [In] IntPtr lpOverlapped);
        }

        void SpoofHWIDserial()
        {
            string newSerial = GenerateString(8);
            uint serial = uint.Parse(newSerial, NumberStyles.HexNumber);
            Log("HDD serial changing to: " + newSerial + " - " + serial);
            ChangeSerialNumber('C', serial);
            Log("");
        }
        #endregion

        #region SpoofPCName
        void SpoofPCName()
        {
            Log("Current PC name: " + CurrentPCName());

            RegistryKey OurKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            OurKey = OurKey.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\ComputerName\ActiveComputerName", true);
            OurKey.SetValue("ComputerName", "DESKTOP-" + GenerateString(15));
            OurKey.Close();

            Log("PC name spoofed to: " + CurrentPCName());
            Log("");
        }

        public static string CurrentPCName()
        {
            string location = @"SYSTEM\CurrentControlSet\Control\ComputerName\ActiveComputerName";
            string name = "ComputerName";

            using (RegistryKey localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey rk = localMachineX64View.OpenSubKey(location))
                {
                    if (rk == null)
                        throw new KeyNotFoundException(string.Format("Key Not Found: {0}", location));

                    object pcname = rk.GetValue(name);
                    if (pcname == null)
                        throw new IndexOutOfRangeException(string.Format("Index Not Found: {0}", name));

                    return pcname.ToString();
                }
            }
        }
        #endregion
    }
}
