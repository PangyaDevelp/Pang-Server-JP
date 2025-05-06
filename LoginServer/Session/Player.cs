﻿using System.Net;
using System.Net.Sockets;
using LoginServer.GameType;
using PangyaAPI.Network.PangyaPacket;
using PangyaAPI.Network.PangyaSession;
using PangyaAPI.Network.PangyaUtil;

namespace LoginServer.Session
{
    public class Player : PangyaAPI.Network.PangyaSession.Session
    {
        public PlayerInfo m_pi { get; set; }

        public Player(pangya_packet_handle_base _threadpool) : base(_threadpool)
        {
            m_pi = new PlayerInfo();
        }


        public override string getNickname()
        {
            return m_pi.nickname;
        }

        public override uint getUID()
        {
            return m_pi.uid;
        }

        public override string getID()
        {
            return m_pi.id;
        }

        public override uint getCapability() { return (uint)m_pi.m_cap; }

        public override bool clear()
        {
            bool ret;
            if ((ret = base.clear()))
            {

                // Player Info
                m_pi.clear();

            }
            return ret;
        }

        public override byte getStateLogged()
        {
            return 1;
        }
    }
}
