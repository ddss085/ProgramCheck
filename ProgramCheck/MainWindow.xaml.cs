using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProgramCheck
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        static string[] prglist, setuplist;
        List<string> allprg, installedprg, uninstalledprg;

        static Dictionary<string, ProgramInformation> prgInfo;
        
        //ProgramInformation[] prgInfo = null;

        public MainWindow()
        {
            //prgInfo = new ProgramInformation[15];
            prgInfo = new Dictionary<string, ProgramInformation>();

            prglist = new string[]
            {
            "Acronis True Image",
            "Adobe Reader 8.1.2",
            "Basler pylon SDK x64 3.2.3.3215",
            "HMI",
            "IntervalZero RTX64 3.4 Runtime",
            "IOGuidePro",
            "LibreOffice 6.1.0.3",
            "Matrox Imaging",
            "Neousys Nuvo/Nuvis/POC Series WDT & DIO 64-bit Library version Ver. 2.2.9",
            "Panasonic Servo MINAS",
            "PANATERM ver.6.0",
            "PowerChute Personal Edition 3.0.2",
            "pylon 5 Camera Software Suite 5.0.5.8999",
            "Realtek USB Wireless LAN Driver",
            "SADP",
            //"Sentinel Runtime",
            "Sentinel System Driver Installer 7.5.9",
            "Soft Servo Systems - Runtime Installer",
            "TeamViewer 11",
            "Touchside",
            //"Universal Pointer Device Driver",
            "VNC Enterprise Edition E4.4.2",
            "VNC Mirror Driver 1.8.0",
            "Vulkan Run Time Libraries 1.0.33.0",
            "WMX2",
            };
            setuplist = new string[]
            {
            "\"D:\\WintechAutomation\\Setup\\39. AcronisTrueImage\\AcronisTrueImage2018_12510.exe\"",
            @"",
            "\"D:\\WintechAutomation\\Setup\\17. Basler pylon SDK\\Basler pylon SDK x64 3.2.3.3215.exe\"",
            @"",
            "\"D:\\WintechAutomation\\Setup\\24. Softservo\\190109_WMX2_v2.2041_RTX3.4_64bit\\190109_WMX2_v2.2041_RTX3.4_64bit\\RTX\\RTX64_3.4_Runtime_Setup.exe\"",
            "\"D:\\WintechAutomation\\Setup\\01. IOGuideProSetup\\IOGuideProSetup.exe\"",
            "\"D:\\WintechAutomation\\Setup\\06. Open Office\\LibreOffice_6.1.0_Win_x64.msi\"",
            "\"D:\\WintechAutomation\\Setup\\28. Mill\\MIL+9+R2+build+1950+du43+pp2(64-bit)\\setup.exe\"",
            "\"D:\\WintechAutomation\\Setup\\41. NeousysTech PC Driver\\NSSetup.exe\"",
            "\"D:\\WintechAutomation\\Setup\\02. Servo Software\03. Panasonic\\파나텀 6.107 최신\\64bit\\setup.exe\"",
            "\"D:\\WintechAutomation\\Setup\\02. Servo Software\03. Panasonic\\파나텀 6.107 최신\\64bit\\setup.exe\"",
            "\"D:\\WintechAutomation\\Setup\\03. UPS\\PCPEInstaller\\PCPEInstaller.exe\"",
            "\"D:\\WintechAutomation\\Setup\\17. Basler pylon SDK\\Basler_pylon_5.0.5.8999.exe\"",
            "\"D:\\WintechAutomation\\Setup\\07. USB Lan Driver\03. A3000U\\RTLWlanU_WindowsDriver_1030.25\\RTLWlanU_WindowsDriver_1030.25\\Setup.exe\"",
            "\"D:\\WintechAutomation\\Setup\\4. 장비 install file\\27. CCTV(HIK VISION)\\WebComponents.exe\"",
            //\\192.168.1.252\m\otion\1\.Robot & Motion\5\3. 제작업무\4. 장비 install fileup\\"",
            "\"D:\\WintechAutomation\\Setup\\40. Sentinel-HL\\Sentinel-System-Driver-Installer-7.5.9_Windows\\Sentinel System Driver Installer 7.5.9.exe\"",
            "\"D:\\WintechAutomation\\Setup\\24. Softservo\\190109_WMX2_v2.2041_RTX3.4_64bit\\190109_WMX2_v2.2041_RTX3.4_64bit\\Runtime\\RuntimeInstaller.exe\"",
            "\"D:\\WintechAutomation\\Setup\\08. TeamViewer_Setup_ko\\TeamViewer_Setup_ko.exe\"",
            "\"D:\\WintechAutomation\\Setup\\04. I view Korea touchmonitor\\setup.exe\"",
            //\\192.168.1.252\m\otion\1\.Robot & Motion\5\3. 제작업무\4. 장비 install fileup\\"",
            "\"D:\\WintechAutomation\\Setup\\18. VNC\vnc-E4_4_2-x86_x64_win32.exe\"",
            "\"D:\\WintechAutomation\\Setup\\18. VNC\vnc-E4_4_2-x86_x64_win32.exe\"",
            "\"D:\\WintechAutomation\\Setup\\41. NeousysTech PC Driver\\Driver_Pool\\Graphics_SKL_APL\\Win_7_8_10_APL_64\\Graphics\\VulkanRT-Installer.exe\"",
            "\"D:\\WintechAutomation\\Setup\\24. Softservo\\190109_WMX2_v2.2041_RTX3.4_64bit\\190109_WMX2_v2.2041_RTX3.4_64bit\\WMX2 Installer.exe\"",
            };

            this.DataContext = new MainWindowViewModel();
            allprg = new List<string>();
            installedprg = new List<string>();
            uninstalledprg = new List<string>();

            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < prglist.Length; i++)
            {
                prgInfo.Add(prglist[i], new ProgramInformation(prglist[i], setuplist[i]));
            }
            
            UpdateLists();
        }

        private void Uninstall_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("정말 삭제하시겠습니까?", "프로그램 삭제", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.None) == MessageBoxResult.Yes)
            {
                SelectedPrg().Delete();
            }
        }
        private void install_Click(object sender, RoutedEventArgs e)
        {
            SelectedPrg().Setup();
        }
        private void InstalledPrgListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateViewBox();
        }

        /// <summary>
        /// 선택된 프로그램 정보를 반환합니다.
        /// </summary>
        private ProgramInformation SelectedPrg()
        {
            foreach (var a in prgInfo.Values)
            {
                if (a.PrgName == ProgramList.SelectedItem as string)
                {
                    return a;
                }
            }
            return null;
        }

        /*
        1.SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall inside CurrentUser
        2.SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall inside LocalMachine
        3.SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall in LocalMachine

        the code below suits you needs.
        */

        /// <summary>
        /// 리스트를 업데이트합니다..
        /// </summary>
        private void UpdateLists()
        {
            ProgramList.ItemsSource = null;
            allprg.Clear();
            installedprg.Clear();
            uninstalledprg.Clear();
            foreach (var a in prgInfo.Values)
            {
                allprg.Add(a.PrgName);
                if (a.IsInstalled())
                {
                    installedprg.Add(a.PrgName);
                }
                else
                {
                    uninstalledprg.Add(a.PrgName);
                }
                a.Set();
            }
            if (AllProgram_radbtn.IsChecked == true)
                ProgramList.ItemsSource = allprg;
            else if (InstalledProgram_radbtn.IsChecked == true)
                ProgramList.ItemsSource = installedprg;
            else if (UninstalledProgram_radbtn.IsChecked == true)
                ProgramList.ItemsSource = uninstalledprg;
            StringBuilder sb = new StringBuilder();
            sb.Append('*', 10);
        }

        /// <summary>
        /// 리뷰 박스를 업데이트합니다.
        /// </summary>
        private void UpdateViewBox()
        {
            List<SubInfo> infos = new List<SubInfo>();

            if (SelectedPrg() != null)
            {
                infos.Add(new SubInfo()
                {
                    Key = "설치 유무",
                    Value = SelectedPrg().IsInstalled() ? "설치됨" : "설치안됨"
                });
                infos.Add(new SubInfo()
                {
                    Key = "DisplayName",
                    Value = SelectedPrg().DisplayName
                });
                infos.Add(new SubInfo()
                {
                    Key = "DisplayVersion",
                    Value = SelectedPrg().DisplayVersion
                });
                infos.Add(new SubInfo()
                {
                    Key = "InstallDate",
                    Value = SelectedPrg().InstallDate
                });
                infos.Add(new SubInfo()
                {
                    Key = "Publisher",
                    Value = SelectedPrg().Publisher
                });
                infos.Add(new SubInfo()
                {
                    Key = "UninstallString",
                    Value = SelectedPrg().UninstallString
                });
            }

            Program_View.ItemsSource = infos;

        }


        private void Updatebtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateLists();
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateViewBox();
            Uninstall_Button.IsEnabled = false;
            Install_Button.IsEnabled = false;
            if (SelectedPrg() != null)
            {
                if (SelectedPrg().IsInstalled())
                {
                    Uninstall_Button.IsEnabled = true;
                    Install_Button.IsEnabled = false;
                }
                else
                {
                    Uninstall_Button.IsEnabled = false;
                    Install_Button.IsEnabled = true;
                }
            }
        }

        private void PrgSetbtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        private void Allbtn_Checked(object sender, RoutedEventArgs e)
        {
            ProgramList.ItemsSource = allprg;
        }
        private void Installedbtn_Checked(object sender, RoutedEventArgs e)
        {
            ProgramList.ItemsSource = installedprg;
        }
        private void Uninstalledbtn_Checked(object sender, RoutedEventArgs e)
        {
            ProgramList.ItemsSource = uninstalledprg;
        }
        


        //드라이버 위치
        //HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\RealtekWlanU

    }

    public class SubInfo
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
