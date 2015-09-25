using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication_CQRestAPI.Models.Tool.Token
{
    public class DeivceSelected
    {
        //private static UserDeviceMutability_mobile _instance_mobile;
        //private static UserDeviceMutability_pc _instance_pc;
        //private static UserDeviceMutability_other _instance_other;

        static public UserDeviceMutability GetDeivceInstance(DeviceType dt) {

            switch (dt) {
                case DeviceType.Mobile:
                    return UserDeviceMutability_mobile.GetInstance();
                case DeviceType.PC:
                    return UserDeviceMutability_pc.GetInstance();
                default:
                    return UserDeviceMutability_mobile.GetInstance();
                        }
        } 
    }
}