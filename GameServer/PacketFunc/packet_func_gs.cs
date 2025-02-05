﻿using GameServer.Game;
using GameServer.Game.Manager;
using GameServer.PangType;
using GameServer.Session;
using PangyaAPI.Network.Cmd;
using PangyaAPI.Network.Pangya_St;
using PangyaAPI.Network.PangyaPacket;
using PangyaAPI.Utilities;
using PangyaAPI.Utilities.BinaryModels;
using PangyaAPI.Utilities.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using _smp = PangyaAPI.Utilities.Log;
using packet = PangyaAPI.Network.PangyaPacket.Packet;
using SYSTEMTIME = GameServer.PangType.PangyaTime;
using static GameServer.PangType._Define;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace GameServer.PacketFunc
{
    public static class packet_func
    {
        static int MAX_BUFFER_PACKET = 1000;
        
        static GameServerTcp.GameServer gs = Program.gs;
        public static void packet002(ParamDispatch _arg1)
        {
            try
            {
                gs.requestLogin((Player)_arg1._session, _arg1._packet);
            }
            catch (exception ex)
            {
                Console.WriteLine($"[packet_func_gs::packet002][StError]: {ex.getFullMessageError()}");
            }
        }

        public static void packet003(ParamDispatch pd)
        {

            try
            {

                gs.requestChat((Player)pd._session, pd._packet);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[packet_func_gs::packet003][StError]: {ex.Message}");
            }
        }

        public static void packet004(ParamDispatch pd)
        {
            try
            {
                // Enter Channel, channel ID
                gs.requestEnterChannel((Player)pd._session, pd._packet);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[packet_func_gs::packet004][StError]: {ex.Message}");
            }
        }

        public static void packet006(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestFinishGame(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet006][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet007(ParamDispatch pd)
        {

            //NICK_CHECK nc = NICK_CHECK.SUCCESS;
            //uint error_info = 0;

            //string nick = "";

            //byte opt = 0;
            //byte error = 2;

            //MemberInfoEx mi = new MemberInfoEx();

            //try
            //{

            //    // Verifica se session está autorizada para executar esse ação,
            //    // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
            //    if (!((Player)pd._session).m_is_authorized)
            //    {
            //        throw new exception("[packet_func::" + "packet007" + "][Error] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Nao esta autorizado a fazer esse request por que ele ainda nao fez o login com o Server. Hacker ou Bug", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
            //            1, 0x7000501));
            //    }

            //    opt = pd._packet.ReadByte();

            //    if (opt != 0)
            //    {
            //        _smp.message_pool.push(new message("[packet_func::packet007][WARNING] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Pediu para Check Nickname: " + nick + ", [OPT=" + Convert.ToString(opt) + "] diferente de 0.", type_msg.CL_FILE_LOG_AND_CONSOLE));
            //    }

            //    nick = pd._packet.ReadPString();

            //    _smp.message_pool.push(new message("[packet_func::packet007][Log] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Pediu para Check Nickname: " + nick, type_msg.CL_FILE_LOG_AND_CONSOLE));

            //    if (nc == SUCCESS && std::regex_match(nick.GetEnumerator(),
            //        nick.end(),
            //        std::wregex(".*[ ].*")))
            //    {
            //        nc = EMPETY_ERROR;

            //        _smp.message_pool.push(new message("[packet_func::packet007][Log] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Pediu para verificar o nick contem espaco em branco: " + nick, type_msg.CL_FILE_LOG_AND_CONSOLE));
            //    }

            //    // C++ TO C# CONVERTER TASK: There is no equivalent to escaping the '?' character in C#:
            //    if (nc == SUCCESS
            //        && nick.Length < 4
            //        || std::regex_match(nick.GetEnumerator(),
            //            nick.end(),
            //            std::wregex(".*[\\^$&,\\?`´~\\|\"@#¨'%*!\\\\].*")))
            //    {
            //        nc = INCORRECT_NICK;

            //        _smp.message_pool.push(new message("[packet_func::packet007][Log] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Pediu para verificar o nick eh menor que 4 letras ou tem caracteres que nao pode: " + nick, type_msg.CL_FILE_LOG_AND_CONSOLE));
            //    }

            //    if (nc == SUCCESS)
            //    {
            //        CmdVerifNick cmd_vn = new CmdVerifNick(nick, true); // Waiter

            //        NormalManagerDB.add(0,
            //            cmd_vn, null, null);

            //        cmd_vn.ExecCmd();

            //        if (cmd_vn.getException().getCodeError() != 0)
            //        {
            //            throw cmd_vn.getException();
            //        }

            //        if (cmd_vn.getLastCheck())
            //        {
            //            nc = NICK_IN_USE;

            //            error = (byte)(nc == NICK_IN_USE && cmd_vn.getUID() != 0 ? 0 : 2);

            //            CmdMemberInfo cmd_mi = new CmdMemberInfo(cmd_vn.getUID(), true); // Waiter

            //            NormalManagerDB.add(0,
            //                cmd_mi, null, null);

            //            cmd_mi.ExecCmd();

            //            if (cmd_mi.getException().getCodeError() != 0)
            //            {
            //                throw cmd_mi.getException();
            //            }

            //            mi = cmd_mi.getInfo();

            //            _smp.message_pool.push(new message("[packet_func::packet007][Log] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Pediu para verificar o nick ja esta em uso: " + nick, type_msg.CL_FILE_LOG_AND_CONSOLE));
            //        }
            //    }

            //}
            //catch (exception e)
            //{

            //    _smp.message_pool.push(new message("[packet_func::packet007][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

            //    if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) == STDA_ERROR_TYPE.PANGYA_DB)
            //    {
            //        nc = ERROR_DB;
            //    }
            //    else
            //    {
            //        nc = UNKNOWN_ERROR;
            //    }

            //}
            //catch (System.Exception e)
            //{

            //    _smp.message_pool.push(new message("[packet_func::packet007][ErrorSystem] " + e.Message, type_msg.CL_FILE_LOG_AND_CONSOLE));

            //    nc = UNKNOWN_ERROR;
            //}

            //try
            //{

            //    var p = new PangyaBinaryWriter((ushort)0xA1);

            //    p.WriteUint8(error);

            //    if (error == 0 && nc == NICK_IN_USE)
            //    {
            //        p.WriteInt32(mi.uid);

            //        p.WriteBuffer(mi, sizeof(MemberInfo)); // esse aqui no antigo enviar sem o número da sala
            //    }

            //    pd._session.Send(p,
            //        ((Player)pd._session), 1);

            //}
            //catch (exception e)
            //{

            //    _smp.message_pool.push(new message("[packet_func::packet007][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            //}

            //
        }

        enum NICK_CHECK : byte
        {
            SUCCESS, // Sucesso por trocar o nick por que ele está disponível
            UNKNOWN_ERROR, // Erro desconhecido, Error ao verificar NICK
            NICK_IN_USE, // NICKNAME já em uso
            INCORRECT_NICK, // INCORRET nick, tamanho < 4 ou tem caracteres que não pode
            NOT_ENOUGH_COOKIE, // Não tem points suficiente
            HAVE_BAD_WORD, // Tem palavras que não pode no NICK
            ERROR_DB, // Erro DB
            EMPETY_ERROR, // Erro Vazio
            EMPETY_ERROR_2, // ERRO VAZIO 2
            SAME_NICK_USED, // O Mesmo nick vai ser usado, estou usando para o mesmo que o ID
            EMPETY_ERROR_3, // ERRO VAZIO 3
            CODE_ERROR_INFO = 12 // CODE  ERROR INFO arquivo iff, o código do erro para mostra no cliente
        }

        public static void packet008(ParamDispatch pd)
        {



            try
            {

                Channel _channel = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (_channel != null)
                {
                    _channel.requestMakeRoom(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet008][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL) // Por Hora relança qualquer exception que não seja do channel
                {
                    throw;
                }
            }


        }

        public static void packet009(ParamDispatch pd)
        {



            try
            {

                Channel _channel = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (_channel != null)
                {
                    _channel.requestEnterRoom(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet009][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL) // Por Hora relança qualquer exception que não seja do channel
                {
                    throw;
                }
            }


        }

        public static void packet00A(ParamDispatch pd)
        {



            try
            {

                Channel _channel = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (_channel != null)
                {
                    _channel.requestChangeInfoRoom(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet00A][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL) // Por Hora relança qualquer exception que não seja do channel
                {
                    throw;
                }
            }


        }

        public static void packet00B(ParamDispatch pd)
        {



            try
            {

                Channel _channel = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (_channel != null)
                {
                    _channel.requestChangePlayerItemChannel(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet00B][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL) // Por Hora relança qualquer exception que não seja do channel
                {
                    throw;
                }
            }


        }

        public static void packet00C(ParamDispatch pd)
        {



            try
            {

                Channel _channel = gs.findChannel(((Player)pd._session).m_pi.channel);

                // Bloquear para ver se funciona o sync do entra depois no camp,
                // mesmo que o outro(0x9D) chama primeiro esse(0x0C) é mais rápido para verificar se o player está em uma sala
                //

                if (_channel != null)
                {
                    _channel.requestChangePlayerItemRoom(((Player)pd._session), pd._packet);
                }

                // Bloquear para ver se funciona o sync do entra depois no camp,
                // mesmo que o outro(0x9D) chama primeiro esse(0x0C) é mais rápido para verificar se o player está em uma sala
                //

            }
            catch (exception e)
            {

                // Bloquear para ver se funciona o sync do entra depois no camp,
                // mesmo que o outro(0x9D) chama primeiro esse(0x0C) é mais rápido para verificar se o player está em uma sala
                //  

                _smp.message_pool.push(new message("[packet_func::packet00C][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL) // Por Hora relança qualquer exception que não seja do channel
                {
                    throw;
                }
            }


        }

        public static void packet00D(ParamDispatch pd)
        {



            try
            {

                Channel c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangePlayerStateReadyRoom(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet00D][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL) // Por Hora relança qualquer exception que não seja do channel
                {
                    throw;
                }
            }


        }

        public static void packet00E(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestStartGame(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet00E][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet00F(ParamDispatch pd)
        {



            try
            {

                Channel c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestExitRoom(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet00F][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL) // Por Hora relança qualquer exception que não seja do channel
                {
                    throw;
                }
            }


        }

        public static void packet010(ParamDispatch pd)
        {



            try
            {

                Channel c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangePlayerTeamRoom(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet010][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL) // Por Hora relança qualquer exception que não seja do channel
                {
                    throw;
                }
            }


        }

        public static void packet011(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestFinishLoadHole(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet011][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet012(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

#if DEBUG
                _smp.message_pool.push(new message("[packet_func::packet12][Log] request Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "]", type_msg.CL_FILE_LOG_AND_CONSOLE));
#endif // _DEBUG

                if (c != null)
                {
                    c.requestInitShot(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet012][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet013(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangeMira(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet013][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet014(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangeStateBarSpace(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet014][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet015(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActivePowerShot(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet015][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet016(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangeClub(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet016][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet017(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestUseActiveItem(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet017][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet018(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangeStateTypeing(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet018][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet019(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestMoveBall(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet019][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet01A(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestInitHole(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet01A][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet01B(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

#if DEBUG
                _smp.message_pool.push(new message("[packet_func::packet1B][Log] request Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "]", type_msg.CL_FILE_LOG_AND_CONSOLE));
#endif // _DEBUG

                if (c != null)
                {
                    c.requestSyncShot(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet01B][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet01C(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestFinishShot(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet01C][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet01D(ParamDispatch pd)
        {



            try
            {

                Channel c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestBuyItemShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet01D][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL) // Por Hora relança qualquer exception que não seja do channel
                {
                    throw;
                }
            }


        }

        public static void packet01F(ParamDispatch pd)
        {



            try
            {

                Channel c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestGiftItemShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet01F][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL) // Por Hora relança qualquer excpetion que não seja do channel
                {
                    throw;
                }
            }


        }

        public static void packet020(ParamDispatch pd)
        {



            try
            {

                Channel c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangePlayerItemMyRoom(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet020][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL) // Por Hora relança qualquer exception que não seja do channel
                {
                    throw;
                }
            }


        }

        public static void packet022(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestStartTurnTime(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet022][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet026(ParamDispatch pd)
        {



            try
            {

                Channel c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestKickPlayerOfRoom(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet026][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));


            }


        }

        public static void packet029(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCheckInvite(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet029][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet02A(ParamDispatch pd)
        {



            try
            {

                gs.requestPrivateMessage(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet02A][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet02D(ParamDispatch pd)
        {



            try
            {

                Channel c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestShowInfoRoom(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet02D][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));


            }


        }

        public static void packet02F(ParamDispatch pd)
        {



            try
            {

                gs.requestPlayerInfo(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet02F][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet030(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestUnOrPauseGame(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet030][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet031(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestFinishHoleData(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet031][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet032(ParamDispatch pd)
        {



            try
            {

                Channel c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangePlayerStateAFKRoom(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet032][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet033(ParamDispatch pd)
        {



            try
            {

                gs.requestExceptionClientMessage(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet033][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet034(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestFinishCharIntro(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet034][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet035(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestTeamFinishHole(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet035][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet036(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestReplyContinueVersus(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet036][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet037(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestLastPlayerFinishVersus(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet037][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet039(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestPayCaddieHolyDay(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet039][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet03A(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestPlayerReportChatGame(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet03A][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        // 2018-03-04 19:26:39.633	Tipo: 60(0x3C), desconhecido ou nao implementado. func_arr::getPacketCall()	 Error Code: 335609856
        // 2018-03-04 19:26:39.633	size packet: 4
        //
        //0000 3C 00 1F 01 -- -- -- -- -- -- -- -- -- -- -- -- 	<...............
        //static int packet03C(void* _arg1, void* _arg2);	// manda msg OFF na opção 0x6F e a opção 0x11F pede a lista de amigos para enviar presente

        public static void packet03C(ParamDispatch pd)
        {



            try
            {

                gs.requestTranslateSubPacket(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet03C][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet03D(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCookie(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet03D][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet03E(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestEnterSpyRoom(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet03E][ErrorSystem] " + e.getCodeError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet041(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestExecCCGIdentity(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet041][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet042(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestInitShotArrowSeq(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet042][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet043(ParamDispatch pd)
        {



            try
            {

                gs.sendServerListAndChannelListToSession(((Player)pd._session));

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet043][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet047(ParamDispatch pd)
        {
            try
            {

                gs.sendRankServer(((Player)pd._session));

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet047][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet048(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestLoadGamePercent(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet048][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet04A(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveReplay(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet04A][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet04B(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestClubSetStatsUpdate(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet04B][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet04F(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangeStateChatBlock(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet04F][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet054(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChatTeam(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet054][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet055(ParamDispatch pd)
        {



            try
            {

                gs.requestChangeWhisperState(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet055][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet057(ParamDispatch pd)
        {



            try
            {

                gs.requestCommandNoticeGM(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet057][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet05C(ParamDispatch pd)
        {



            try
            {

                gs.sendDateTimeToSession(((Player)pd._session));

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet05C][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        // 2018 - 12 - 01 18:49 : 14.928 size packet : 4
        // Destroy Room, 2 Bytes Room Number
        // 0000 60 00 01 00 -- -- -- -- -- -- -- -- -- -- -- --    `...............
        public static void packet060(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestExecCCGDestroy(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet060][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        // 2018 - 12 - 01 18:48 : 02.634 size packet : 6
        // Disconnect User, 2 Bytes Online ID
        // 0000 61 00 00 00 00 00 -- -- -- -- -- -- -- -- -- --a...............
        public static void packet061(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    _smp.message_pool.push(new message("[packet_func::packet061][Log] player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] tentou desconectar um player, mas o server ja faz o tratamento do packet08F do comando GM.", type_msg.CL_FILE_LOG_AND_CONSOLE));
                }

                // Verifica se session está autorizada para executar esse ação,
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                if (!((Player)pd._session).m_is_authorized)
                {
                    //throw new exception("[packet_func::" + "packet061" + "][Error] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Nao esta autorizado a fazer esse request por que ele ainda nao fez o login com o Server. Hacker ou Bug", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
                    //    1, 0x7000501));
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet061][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet063(ParamDispatch pd)
        {



            try
            {

                Channel c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestPlayerLocationRoom(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet063][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));


            }


        }

        public static void packet064(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestDeleteActiveItem(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet064][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet065(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveBooster(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet065][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet066(ParamDispatch pd)
        {



            try
            {

                gs.requestSendTicker(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet066][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet067(ParamDispatch pd)
        {



            try
            {

                gs.requestQueueTicker(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet067][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet069(ParamDispatch pd)
        {



            try
            {

                gs.requestChangeChatMacroUser(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet069][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet06B(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestSetNoticeBeginCaddieHolyDay(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet06B][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet073(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangeMascotMessage(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet73][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet074(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCancelEditSaleShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet074][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet075(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCloseSaleShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet075][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet076(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestOpenEditSaleShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet076][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet077(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestViewSaleShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet077][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet078(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCloseViewSaleShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet078][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet079(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangeNameSaleShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet079][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet07A(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestVisitCountSaleShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet07A][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet07B(ParamDispatch pd)
        {



            try
            {

                var r = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (r != null)
                {
                    r.requestPangSaleShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet07B][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet07C(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestOpenSaleShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet07C][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet07D(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestBuyItemSaleShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet07D][Error] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet081(ParamDispatch pd)
        {



            try
            {

#if DEBUG
                _smp.message_pool.push(new message("[packet_func::packet081][Log] Hex:\n\r" + pd._packet.Log(), type_msg.CL_FILE_LOG_AND_CONSOLE));
#endif // _DEBUG

                Channel c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestEnterLobby(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet081][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet082(ParamDispatch pd)
        {



            try
            {

#if DEBUG
                _smp.message_pool.push(new message("[packet_func::packet082][Log] Hex:\n\r" + pd._packet.Log(), type_msg.CL_FILE_LOG_AND_CONSOLE));
#endif // _DEBUG

                Channel c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestExitLobby(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet082][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

            }


        }

        public static void packet083(ParamDispatch pd)
        {



            try
            {

#if DEBUG
                _smp.message_pool.push(new message("[packet_func::packet083][Log] Hex:\n\r" + pd._packet.Log(), type_msg.CL_FILE_LOG_AND_CONSOLE));
#endif // _DEBUG

                gs.requestEnterOtherChannelAndLobby(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet083][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet088(ParamDispatch pd)
        {



            try
            {

                gs.requestCheckGameGuardAuthAnswer(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet088][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet08B(ParamDispatch pd)
        {



            try
            {

                // Verifica se session está autorizada para executar esse ação,
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                if (!((Player)pd._session).m_is_authorized)
                {
                    throw new exception("[packet_func::" + "packet08B" + "][Error] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Nao esta autorizado a fazer esse request por que ele ainda nao fez o login com o Server. Hacker ou Bug", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
                        1, 0x7000501));
                }

                CmdServerList cmd_sl = new CmdServerList(TYPE_SERVER.MSN); // waitable


                cmd_sl.ExecCmd();

                if (cmd_sl.getException().getCodeError() != 0)
                {
                    _smp.message_pool.push(new message("[packet_func::packet08B][Error] " + cmd_sl.getException().getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
                }

                var v_si = cmd_sl.getServerList();
                //std::vector< ServerInfo > v_si = pangya_db::getMSNServer();

                //var p =  pacote0FC(p,
                //    ((Player)pd._session), v_si);
                //pd._session.Send(p,
                //    ((Player)pd._session), 0);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet08B][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }

        public static void packet08F(ParamDispatch pd)
        {



            try
            {

                gs.requestCommonCmdGM(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet08F][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet098(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestOpenPapelShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet098][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet09C(ParamDispatch pd)
        {



            try
            {

                // Verifica se session está autorizada para executar esse ação,
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                if (!((Player)pd._session).m_is_authorized)
                {
                    throw new exception("[packet_func::" + "packet09C(Last5Player)" + "][Error] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Nao esta autorizado a fazer esse request por que ele ainda nao fez o login com o Server. Hacker ou Bug", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
                        1, 0x7000501));
                }

                //// Last 5 Player Game Info
                //var p = new PangyaBinaryWriter();

                //pacote10E(p,
                //    ((Player)pd._session),
                //    ((Player)pd._session).m_pi.l5pg);
                //pd._session.Send(p,
                //    ((Player)pd._session), 1);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet09C][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }

        public static void packet09D(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                // Bloquear para ver se funciona o sync do entra depois no camp,
                // mesmo que o outro(0x9D) chama primeiro esse(0x0C) é mais rápido para verificar se o player está em uma sala


                if (c != null)
                {
                    c.requestEnterGameAfterStarted(((Player)pd._session), pd._packet);
                }

                // Bloquear para ver se funciona o sync do entra depois no camp,
                // mesmo que o outro(0x9D) chama primeiro esse(0x0C) é mais rápido para verificar se o player está em uma sala


            }
            catch (exception e)
            {

                // Bloquear para ver se funciona o sync do entra depois no camp,
                // mesmo que o outro(0x9D) chama primeiro esse(0x0C) é mais rápido para verificar se o player está em uma sala


                _smp.message_pool.push(new message("[packet_func::packet09D][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet09E(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestUpdateGachaCoupon(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet09E][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0A1(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestEnterWebLinkState(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0A1][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0A2(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestExitedFromWebGuild(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0A2][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0AA(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestUseTicketReport(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0AA][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0AB(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestOpenTicketReportScroll(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0AB][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0AE(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestMakeTutorial(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0AE][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0B2(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestOpenBoxMyRoom(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0B2][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0B4(ParamDispatch pd)
        {



            try
            {

                // Esse pacote é que o player aceitou convite do player entrou na sala saiu e relogou, ai manda esse pacote com o número da sala
                /*_smp::message_pool::push(new message("[packet_func::packet0B4][Log] Player[UID=" + std::to_string(((Player)pd._session).m_pi.uid)
                        + "] mandou o Pacote0B4 mas nao sei o que ele pede ou faz ainda. Hex: \n\r"
                        + hex_util::BufferToHexString(pd._packet->getBuffer(), pd._packet->getSize()), type_msg.CL_FILE_LOG_AND_CONSOLE));*/

                // Verifica se session está autorizada para executar esse ação,
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                if (!((Player)pd._session).m_is_authorized)
                {
                    throw new exception("[packet_func::" + "packet0B4" + "][Error] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Nao esta autorizado a fazer esse request por que ele ainda nao fez o login com o Server. Hacker ou Bug", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
                        1, 0x7000501));
                }

                byte option = pd._packet.ReadByte();
                ushort numero_sala = pd._packet.ReadUInt16();

                // Log
                _smp.message_pool.push(new message("[packet_func::packet0B4][Log][Option=" + Convert.ToString((ushort)option) + "] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] foi convidado por um player aceitou o pedido saiu da sala[NUMERO=" + Convert.ToString(numero_sala) + "] e relogou.", type_msg.CL_FILE_LOG_AND_CONSOLE));

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0B4][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0B5(ParamDispatch pd)
        {



            try
            {

                // Verifica se session está autorizada para executar esse ação,
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                if (!((Player)pd._session).m_is_authorized)
                {
                    throw new exception("[packet_func::" + "packet0B5(MyrRoomHouseInfo)" + "][Error] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Nao esta autorizado a fazer esse request por que ele ainda nao fez o login com o Server. Hacker ou Bug", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
                        1, 0x7000501));
                }

                uint from_uid = new uint();
                uint to_uid = new uint();

                from_uid = pd._packet.ReadUInt32();
                to_uid = pd._packet.ReadUInt32();

                var p = new PangyaBinaryWriter();
                p.init_plain((ushort)0x12B);

                // Aqui o player só pode pedir para entrar no dele mesmo
                if (from_uid == to_uid && ((Player)pd._session).m_pi.mrc.allow_enter == 1)
                { // Isso tinha no season 4, agora nos season posteriores tiraram isso
                    p.WriteUInt32(1); // option;

                    p.WriteUInt32(to_uid);

                    p.WriteBuffer(((Player)pd._session).m_pi.mrc, Marshal.SizeOf(new MyRoomConfig()));
                }
                else
                {
                    p.WriteUInt32(0);

                    p.WriteUInt32(to_uid);
                }

                pd._session.Send(p);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0B5][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }

        public static void packet0B7(ParamDispatch pd)
        {



            try
            {

                // Verifica se session está autorizada para executar esse ação,
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                if (!((Player)pd._session).m_is_authorized)
                {
                    throw new exception("[packet_func::" + "packet0B7(InfoPlayerMyRoom)" + "][Error] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Nao esta autorizado a fazer esse request por que ele ainda nao fez o login com o Server. Hacker ou Bug", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
                        1, 0x7000501));
                }

                // @@!! Ajeitar para pegar a função estática da sala que initializa o Player Room Info
                PlayerRoomInfoEx pri = new PlayerRoomInfoEx();

                // Player Room Info Init
                pri.oid = ((Player)pd._session).m_oid;
                pri.position = 0; // posição na sala
                pri.capability = ((Player)pd._session).m_pi.m_cap;
                pri.title = ((Player)pd._session).m_pi.ue.skin_typeid[5];

                if (((Player)pd._session).m_pi.ei.char_info != null)
                {
                    pri.char_typeid = ((Player)pd._session).m_pi.ei.char_info._typeid;
                }

                pri.skin[4] = 0; // Aqui tem que ser zero, se for outro valor não mostra a imagem do character equipado

                //pri.state_flag.uFlag.stFlagBit.master = 1;
                //pri.state_flag.uFlag.stFlagBit.ready = 1; // Sempre está pronto(ready) o master

                //pri.state_flag.uFlag.stFlagBit.sexo = ((Player)pd._session).m_pi.mi.sexo;

                // Só faz calculo de Quita rate depois que o player
                // estiver no level Beginner E e jogado 50 games
                if (((Player)pd._session).m_pi.level >= 6 && ((Player)pd._session).m_pi.ui.jogado >= 50)
                {
                    float rate = ((Player)pd._session).m_pi.ui.getQuitRate();

                    if (rate < GOOD_PLAYER_ICON)
                    {
                        // pri.state_flag.uFlag.stFlagBit.azinha = 1;
                    }
                    else if (rate >= QUITER_ICON_1 && rate < QUITER_ICON_2)
                    {
                        // pri.state_flag.uFlag.stFlagBit.quiter_1 = 1;
                    }
                    else if (rate >= QUITER_ICON_2)
                    {
                        //  pri.state_flag.uFlag.stFlagBit.quiter_2 = 1;
                    }
                }

                pri.level = (byte)((Player)pd._session).m_pi.mi.level;

                if (((Player)pd._session).m_pi.ei.char_info != null && ((Player)pd._session).m_pi.ui.getQuitRate() < GOOD_PLAYER_ICON)
                {
                    pri.icon_angel = ((Player)pd._session).m_pi.ei.char_info.AngelEquiped();
                }
                else
                {
                    pri.icon_angel = 0;
                }

                pri.ucUnknown_0A = 10; // 0x0A dec"10" _session.m_pi.place
                pri.guild_uid = ((Player)pd._session).m_pi.gi.uid;
                //pri.guild_mark_index = ((Player)pd._session).m_pi.gi.index_mark_emblem;
                pri.uid = ((Player)pd._session).m_pi.uid;
                pri.state_lounge = ((Player)pd._session).m_pi.state_lounge;
                pri.usUnknown_flg = 0;
                pri.state = ((Player)pd._session).m_pi.state;
                pri.location = new PlayerRoomInfo.stLocation() { x = ((Player)pd._session).m_pi.location.x, z = ((Player)pd._session).m_pi.location.z, r = ((Player)pd._session).m_pi.location.r };
                pri.shop = new PlayerRoomInfo.PersonShop();

                if (((Player)pd._session).m_pi.ei.mascot_info != null)
                {
                    pri.mascot_typeid = ((Player)pd._session).m_pi.ei.mascot_info._typeid;
                }

                pri.flag_item_boost = ((Player)pd._session).m_pi.checkEquipedItemBoost();
                pri.ulUnknown_flg = 0;
                //pri.id_NT não estou usando ainda
                //pri.ucUnknown106
                pri.convidado = 0; // Flag Convidado, [Não sei bem por que os que entra na sala normal tem valor igual aqui, já que é flag de convidado waiting], Valor constante da sala para os players(ACHO)
                pri.avg_score = ((Player)pd._session).m_pi.ui.getMediaScore();
                //pri.ucUnknown3

                //if (((Player)pd._session).m_pi.ei.char_info != null)
                //{
                //    pri.ci = *((Player)pd._session).m_pi.ei.char_info;
                //}

                //var p = new PangyaBinaryWriter((ushort)0x168); // Character Equipado

                //p.WriteBuffer(pri, sizeof(PlayerRoomInfoEx));

                //pd._session.Send(p,
                //    ((Player)pd._session), 0);

                //p.init_plain((ushort)0x12D); // Itens do Myroom, Mala, Email, sofa, teto chao, e poster, "NESSA SEASON, SÓ USA POSTER"

                //p.WriteUInt32(1); // Option

                //p.WriteUint16((ushort)((Player)pd._session).m_pi.v_mri.size());

                //for (var i = 0u; i < ((Player)pd._session).m_pi.v_mri.size(); ++i)
                //{
                //    p.WriteBuffer(((Player)pd._session).m_pi.v_mri[i], sizeof(MyRoomItem));
                //}

                //pd._session.Send(p,
                //    ((Player)pd._session), 1);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0B7][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }

        public static void packet0B9(ParamDispatch pd)
        {



            try
            {

                gs.requestUCCSystem(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0B9][ErrorSystem]", type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet0BA(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestInvite(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0BA][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0BD(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestUseCardSpecial(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0BD][ErrorSytem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0C1(ParamDispatch pd)
        {



            try
            {

                // Verifica se session está autorizada para executar esse ação,
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                if (!((Player)pd._session).m_is_authorized)
                {
                    throw new exception("[packet_func::" + "packet0C1(UpdatePlace)" + "][Error] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Nao esta autorizado a fazer esse request por que ele ainda nao fez o login com o Server. Hacker ou Bug", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
                        1, 0x7000501));
                }

                ((Player)pd._session).m_pi.place = pd._packet.ReadByte(); // Att place(lugar)

                // Update Location Player on DB
                ((Player)pd._session).m_pi.updateLocationDB();

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0C1][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }

        public static void packet0C9(ParamDispatch pd)
        {



            try
            {

                gs.requestUCCWebKey(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0C9][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet0CA(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestOpenCardPack(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0CA][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0CB(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestFinishGame(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0CB][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0CC(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCheckDolfiniLockerPass(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0CC][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0CD(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestDolfiniLockerItem(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0CD][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0CE(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestAddDolfiniLockerItem(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0CE][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0CF(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestRemoveDolfiniLockerItem(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0CF][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0D0(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestMakePassDolfiniLocker(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0D0][Error] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0D1(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangeDolfiniLockerPass(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {
                _smp.message_pool.push(new message("[packet_func::packet0D1][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0D2(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangeDolfiniLockerModeEnter(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0D2][ErroSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0D3(ParamDispatch pd)
        {                    
            try
            {

                // Verifica se session está autorizada para executar esse ação,
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                if (!((Player)pd._session).m_is_authorized)
                {
                    throw new exception("[packet_func::" + "packet0D3(CheckDolfiniLocker)" + "][Error] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Nao esta autorizado a fazer esse request por que ele ainda nao fez o login com o Server. Hacker ou Bug", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
                        1, 0x7000501));
                }

                uint check = 0u;

                check = ((Player)pd._session).m_pi.df.isLocker();

                var p = new PangyaBinaryWriter((ushort)0x170);

                p.WriteUInt32(0); // option
                p.WriteUInt32(check);

                ((Player)pd._session).Send(p);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0D3][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }

        public static void packet0D4(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestUpdateDolfiniLockerPang(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0D4][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0D5(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestDolfiniLockerPang(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0D5][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0D8(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestUseItemBuff(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0D8][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0DE(ParamDispatch pd)
        {



            try
            {

                // Player não pode ver a message privada que o player mandou, avisa para o server
                /*_smp::message_pool::push(new message("[packet_func::packet0DE][Log] Player[UID=" + std::to_string(((Player)pd._session).m_pi.uid)
                        + "] mandou o Pacote0DE mas nao sei o que ele pede ou faz ainda. Hex: \n\r"
                        + hex_util::BufferToHexString(pd._packet->getBuffer(), pd._packet->getSize()), type_msg.CL_FILE_LOG_AND_CONSOLE));*/

                // Envia mensagem para o player que enviou o MP que o player não pode ver a mensagem
                gs.requestNotifyNotDisplayPrivateMessageNow(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0DE][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.GAME_SERVER)
                {
                    throw;
                }
            }


        }

        public static void packet0E5(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveCutin(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0E5][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0E6(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestExtendRental(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0E6][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0E7(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestDeleteRental(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0E7][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0EB(ParamDispatch pd)
        {



            try
            {

                Channel c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestPlayerStateCharacterLounge(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0EB][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL) // Por Hora relança qualquer exception que não seja do channel
                {
                    throw;
                }
            }


        }

        public static void packet0EC(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCometRefill(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0EC][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0EF(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestOpenBoxMail(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0EF][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet0F4(ParamDispatch pd)
        {



            try
            {
                // TTL = Time To Live    

                // Verifica se session está autorizada para executar esse ação,
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                if (!((Player)pd._session).m_is_authorized)
                {
                    throw new exception("[packet_func::" + "packet0F4" + "][Error] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Nao esta autorizado a fazer esse request por que ele ainda nao fez o login com o Server. Hacker ou Bug", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
                        1, 0x7000501));
                }

                ((Player)pd._session).m_tick_bot = Environment.TickCount;

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0F4][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }

        public static void packet0FB(ParamDispatch pd)
        {



            var p = new PangyaBinaryWriter();

            try
            {

                // Verifica se session está autorizada para executar esse ação,
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                if (!((Player)pd._session).m_is_authorized)
                {
                    throw new exception("[packet_func::" + "packet0FB(WebKey)" + "][Error] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Nao esta autorizado a fazer esse request por que ele ainda nao fez o login com o Server. Hacker ou Bug", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
                        1, 0x7000501));
                }

                //CmdGeraWebKey cmd_gwk = new CmdGeraWebKey(((Player)pd._session).m_pi.uid, true);

                //cmd_gwk.ExecCmd();

                //if (cmd_gwk.getException().getCodeError() != 0)
                //{
                //    throw cmd_gwk.getException();
                //}

                //var webKey = cmd_gwk.getKey();

                //pacote1AD(p,
                //    ((Player)pd._session), webKey, 1);
                //pd._session.Send(p,
                //    ((Player)pd._session), 1);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0FB][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                // Error Reply
                p.init_plain((ushort)0x1AD);

                p.WriteUInt32(0); // Error
                p.WriteUInt16(0); // size Key

                pd._session.Send(p);
            }


        }

        public static void packet0FE(ParamDispatch pd)
        {

            //packet 254, no send response!

            try
            {
                // Verifica se session está autorizada para executar esse ação,
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                if (!((Player)pd._session).m_is_authorized)
                {
                    throw new exception("[packet_func::" + "packet0FE" + "][Error] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Nao esta autorizado a fazer esse request por que ele ainda nao fez o login com o Server. Hacker ou Bug", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
                        1, 0x7000501));
                }

                var p = new PangyaBinaryWriter();

                p.init_plain((ushort)0x1B1);

                p.WriteInt32(0x0132DC55);
                p.WriteByte(0x19);
                p.WriteZero(6);
                p.WriteInt16(0x2211);
                p.WriteZero(17);
                p.WriteByte(0x11);//@@@@@ aqui diz que esta compresss
                p.WriteInt16(0);
                //pd._session.SafeSend(p.GetBytes);//tem que ser sem compress, se nao o projectg envia pacotes estranhos!   
            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet0FE][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }

        public static void packet119(ParamDispatch pd)
        {
            var p = new PangyaBinaryWriter();

            try
            {

                gs.requestChangeServer(((Player)pd._session), pd._packet);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet119][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }

        public static void packet126(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestOpenLegacyTikiShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet126][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw; // Relança
                }
            }


        }

        public static void packet127(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestPointLegacyTikiShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet127][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw; // Relança
                }
            }


        }

        public static void packet128(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestExchangeTPByItemLegacyTikiShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet128][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw; // Relança
                }
            }


        }

        public static void packet129(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestExchangeItemByTPLegacyTikiShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet129][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw; // Relança
                }
            }


        }

        public static void packet12C(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestFinishGame(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet12C][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet12D(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestReplyInitialValueGrandZodiac(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet12D][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet12E(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestMarkerOnCourse(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet12E][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet12F(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

#if DEBUG
                _smp.message_pool.push(new message("[packet_func::packet12F][Log] request Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "]", type_msg.CL_FILE_LOG_AND_CONSOLE));
#endif // _DEBUG

                if (c != null)
                {
                    c.requestShotEndData(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet12F][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet130(ParamDispatch pd)
        {



            var p = new PangyaBinaryWriter();

            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestLeavePractice(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet130][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }

        public static void packet131(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestLeaveChipInPractice(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet131][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet137(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestStartFirstHoleGrandZodiac(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet137][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet138(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveWing(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet138][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet140(ParamDispatch pd)
        {



            try
            {

                // Verifica se session está autorizada para executar esse ação,
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                if (!((Player)pd._session).m_is_authorized)
                {

                }

                var p = new PangyaBinaryWriter((ushort)0x20E);

                p.Write(0);
                p.Write(0); // Não sei pode ACHO "ser Value acho, ou erro, pode ser dizendo que o shop esta bloqueado"

                pd._session.Send(p);

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet140][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }

        public static void packet141(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestChangeWindNextHoleRepeat(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet141][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet143(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestOpenMailBox(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet143][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet144(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestInfoMail(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet144][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet145(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestSendMail(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet145][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet146(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

#if DEBUG
                _smp.message_pool.push(new message("[packet_func::packet146][Log] Request player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "]", type_msg.CL_FILE_LOG_AND_CONSOLE));
#endif // _DEBUG

                if (c != null)
                {
                    c.requestTakeItemFomMail(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet146][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) == STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet147(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestDeleteMail(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet147][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet14B(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestPlayPapelShop(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet14B][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet151(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestDailyQuest(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet151][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet152(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestAcceptDailyQuest(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet152][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet153(ParamDispatch pd)
        {




            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestTakeRewardDailyQuest(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet153][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet154(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestLeaveDailyQuest(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet154][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet155(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestLoloCardCompose(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet155][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet156(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveAutoCommand(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet156][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet157(ParamDispatch pd)
        {



            var p = new PangyaBinaryWriter();

            try
            {

                uint uid = pd._packet.ReadUInt32();

                // Log
#if DEBUG
                _smp.message_pool.push(new message("[packet_func::packet157][Log] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "].\tPlayer Achievement request uid: " + Convert.ToString(uid), type_msg.CL_FILE_LOG_AND_CONSOLE));
#else
					_smp.message_pool.push(new message("[packet_func::packet157][Log] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "].\tPlayer Achievement request uid: " + Convert.ToString(uid), CL_ONLY_FILE_LOG));
#endif // _DEBUG

                //// Verifica se session está autorizada para executar esse ação,
                //// se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                //if (!((Player)pd._session).m_is_authorized)
                //{
                //    throw new exception("[packet_func::" + "packet157(requestAchievementInfo)" + "][Error] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] Nao esta autorizado a fazer esse request por que ele ainda nao fez o login com o Server. Hacker ou Bug", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
                //        1, 0x7000501));
                //}

                //MgrAchievement mgr_achievement = null;
                //player s = null;

                //if (((Player)pd._session).m_pi.uid == uid) // O player solicitou o próprio achievement info
                //{
                //    mgr_achievement = ((Player)pd._session).m_pi.mgr_achievement;
                //}
                //else if ((s = gs.findPlayer(uid)) != null) // O player solicitou o achievement info de outro player online
                //{
                //    mgr_achievement = s.m_pi.mgr_achievement;
                //}
                //else
                //{ // O player solicitou o achievement info de outro player off-line

                //    MgrAchievement mgr_achievement = new MgrAchievement();

                //    mgr_achievement.initAchievement(new uint(uid));

                //    mgr_achievement.sendAchievementGuiToPlayer(((Player)pd._session));

                //    
                //}

                //if (mgr_achievement == null)
                //{
                //    pacote22C(p,
                //        ((Player)pd._session), 1);
                //    pd._session.Send(p, // Error
                //        ((Player)pd._session), 1);
                //}
                //else
                //{
                //    mgr_achievement.sendAchievementGuiToPlayer(((Player)pd._session));
                //}

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet157][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                //pacote22C(p,
                //    ((Player)pd._session), 1);
                //pd._session.Send(p, // Error
                //    ((Player)pd._session), 1);
            }


        }

        public static void packet158(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCadieCauldronExchange(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet158][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet15C(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActivePaws(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet15C][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet15D(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveRing(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet15D][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet164(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestClubSetWorkShopUpLevel(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet164][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet165(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestClubSetWorkShopUpLevelConfirm(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet165][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet166(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestClubSetWorkShopUpLevelCancel(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet166][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet167(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestClubSetWorkShopUpRank(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet167][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet168(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestClubSetWorkShopUpRankTransformConfirm(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet168][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet169(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestClubSetWorkShopUpRankTransformCancel(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet169][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet16B(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestClubSetWorkShopRecoveryPts(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet16B][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet16C(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestClubSetWorkShopTransferMasteryPts(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet16C][ErrorSystem] ", type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet16D(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestClubSetReset(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet16D][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet16E(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCheckAttendanceReward(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet16E][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet16F(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestAttendanceRewardLoginCount(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet16F][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet171(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveEarcuff(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet171][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet172(ParamDispatch pd)
        {



            try
            {

                // !@ Log
                _smp.message_pool.push(new message("[packet_func::packet172][Log] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] request open Event Workshop 2013.", type_msg.CL_FILE_LOG_AND_CONSOLE));

                _smp.message_pool.push(new message("[packet_func::packet172][Log] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "]. Packet raw: " + pd._packet.Log(), type_msg.CL_FILE_LOG_AND_CONSOLE));

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet172][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }

        public static void packet176(ParamDispatch pd)
        {



            try
            {

#if DEBUG
                _smp.message_pool.push(new message("[packet_func::packet176][Log] Hex:\n\r" + pd._packet.Log(), type_msg.CL_FILE_LOG_AND_CONSOLE));
#endif // _DEBUG

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestEnterLobbyGrandPrix(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet176][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet177(ParamDispatch pd)
        {



            try
            {

#if DEBUG
                _smp.message_pool.push(new message("[packet_func::packet177][Log] Hex:\n\r" + pd._packet.Log(), type_msg.CL_FILE_LOG_AND_CONSOLE));
#endif // _DEBUG

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestExitLobbyGrandPrix(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet177][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet179(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestEnterRoomGrandPrix(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet179][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet17A(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestExitRoomGrandPrix(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet17A][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet17F(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestPlayMemorial(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet17F][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet180(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveGlove(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet180][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet181(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveRingGround(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet181][ErroSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet184(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestToggleAssist(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet184][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet185(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveAssistGreen(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet185][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet187(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCharacterMasteryExpand(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet187][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));


            }


        }

        public static void packet188(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCharacterStatsUp(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet188][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet189(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCharacterStatsDown(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet189][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet18A(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCharacterCardEquip(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet18A][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet18B(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCharacterCardEquipWithPatcher(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet18B][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet18C(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestCharacterRemoveCard(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet18C][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet18D(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestTikiShopExchangeItem(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet18D][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet192(ParamDispatch pd)
        {



            try
            {

                // !@ Log
                _smp.message_pool.push(new message("[packet_func::packet192][Log] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "] request open Event Arin 2014.", type_msg.CL_FILE_LOG_AND_CONSOLE));

                _smp.message_pool.push(new message("[packet_func::packet192][Log] Player[UID=" + Convert.ToString(((Player)pd._session).m_pi.uid) + "]. Packet raw: " + pd._packet.Log(), type_msg.CL_FILE_LOG_AND_CONSOLE));

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet192][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }

        public static void packet196(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveRingPawsRainbowJP(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet196][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet197(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveRingPowerGagueJP(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet197][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet198(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveRingMiracleSignJP(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet198][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet199(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestActiveRingPawsRingSetJP(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet199][ErrorSystem]", type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        // packet server
        public static void packet_sv4D(ParamDispatch pd)
        {



            //try
            //{

            //    string str_tmp = "Time: " + Convert.ToString((Environment.TickCount - ((Player)pd._session).m_time_start) / (double)CLOCKS_PER_SEC);

            //    ((Player)pd._session).m_time_start = Environment.TickCount;

            //    _smp.message_pool.push(new message(str_tmp, CL_ONLY_FILE_TIME_LOG));

            //}
            //catch (exception e)
            //{

            //    _smp.message_pool.push(new message("[packet_func::packet_sv4D][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            //}


        }

        public static void packet_sv055(ParamDispatch pd)
        {



            try
            {

                var c = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (c != null)
                {
                    c.requestInitShotSended(((Player)pd._session), pd._packet);
                }

            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet_sv055][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));

                if ((STDA_ERROR_TYPE)ExceptionError.STDA_SOURCE_ERROR_DECODE(e.getCodeError()) != STDA_ERROR_TYPE.CHANNEL)
                {
                    throw;
                }
            }


        }

        public static void packet_svRequestInfo(ParamDispatch pd)
        {



            /*if (++((Player)pd._session).m_pi.ri.show == 12/*Show Info*//*)
                pacote089(p,
                    &((Player)pd._session),
                    ((Player)pd._session).m_pi.ri.uid,
                    ((Player)pd._session).m_pi.ri.season);
                pd._session.Send(p, ((Player)pd._session));
            */

        }

        public static void packet_sv22D(ParamDispatch pd)
        {



            var p = new PangyaBinaryWriter();

            // Já enviou o pacote22D com os achievement todo envia o pacote para abrir o GUI
            // pacote22C(p, ((Player)pd._session));
            //pd._session.Send(p, &((Player)pd._session), 0);    

        }

        public static void packet_svFazNada(ParamDispatch pd)
        {



            // Esse pacote é para os pacotes que o server envia para o cliente
            // e não precisa de tratamento depois que foi enviado para o cliente


        }

        public static void packet_svDisconectPlayerBroadcast(ParamDispatch pd)
        {



            try
            {

                Channel _channel = gs.findChannel(((Player)pd._session).m_pi.channel);

                if (_channel != null)
                {
                    _channel.leaveChannel(((Player)pd._session));
                }
            }
            catch (exception e)
            {

                _smp.message_pool.push(new message("[packet_func::packet_svDisconectPlayerBroadcast][ErrorSystem] " + e.getFullMessageError(), type_msg.CL_FILE_LOG_AND_CONSOLE));
            }


        }
                       
        #region Response Packet
        public static byte[] InitialLogin(PlayerInfo pi, ServerInfoEx _si)
        {
            var p = new PangyaBinaryWriter();
            try
            {
                if (pi == null)
                    throw new Exception("Erro PlayerInfo *pi is null. packet_func::InitialLogin()");

                p.WritePStr(_si.version_client);

                // member info
                p.WriteUInt16(ushort.MaxValue);//num the room
                //write struct info player      
                p.WriteBytes(pi.mi.Build());
                // User Info player
                p.WriteUInt32(pi.uid);
                p.WriteBytes(pi.ui.Build());

                // Trofel Info
                p.WriteBytes(pi.ti_current_season.Build());
                // User Equip
                p.WriteBytes(pi.ue.Build());
                #region MapStatic Work 
                //---------------------------- MAP STATISTIC -------------------------------\\
                // Map Statistics Normal
                for (byte st_i = 0; st_i < MS_NUM_MAPS; st_i++)
                {
                    p.WriteBytes(pi.a_ms_normal[st_i].Build());
                }

                // Map Statistics Natural
                for (byte st_i = 0; st_i < MS_NUM_MAPS; st_i++)
                {
                    p.WriteBytes(pi.a_ms_natural[st_i].Build());
                }

                // Map Statistics Grand Prix
                for (byte st_i = 0; st_i < MS_NUM_MAPS; st_i++)
                {
                    p.WriteBytes(pi.a_ms_grand_prix[st_i].Build());
                }

                // Map Statistics Normal for all seasons
                for (int j = 0; j < 9; j++)
                {
                    for (var st_i = 0; st_i < MS_NUM_MAPS; st_i++)        //talvez algum problema aqui!
                    {
                        p.WriteBytes(pi.aa_ms_normal_todas_season[st_i].Build());
                    }
                }
                //---------------------------- MAP STATIC CORRECT -------------------------------\\
                #endregion fim
                //Equiped Items
                p.WriteBytes(pi.ei.Build());
                // Write Time, 16 Bytes
                p.WriteTime();

                // Config do Server
                p.WriteUInt16(2); // Valor padrão, 1 na primeira vez, 2 para logins subsequentes
                p.WriteStruct(pi.mi.papel_shop, pi.mi.papel_shop);
                p.WriteInt32(1); // Valor novo no JP, indicado como 0 em novas contas
                p.WriteUInt64(pi.block_flag.m_flag.ullFlag); // Flag do server para bloquear sistemas
                p.WriteUInt32(pi.ari.counter); // Quantidade de vezes que logou
                p.WriteUInt32(_si.propriedade.ulProperty);

                if (p.GetSize == 12800)
                    Debug.WriteLine("InitialLogin Size Okay");
                else
                    Debug.WriteLine($"InitialLogin Size Bug: Correct = {12800}, Incorrect = {p.GetSize} => packet_func.InitialLogin()");
 
                return p.GetBytes;
            }
            catch (Exception e)
            {
                _smp.message_pool.push("[packet_func_gs::InitialLogin]", e);
                return new byte[0];
            }
        }

        public static byte[] pacote046(List<PlayerCanalInfo> v_element, int option)
        {
            var p = new PangyaBinaryWriter();
            var elements = v_element.Count();

            p.Write(new byte[] { 0x46, 0x00 });
            p.WriteByte((byte)option);
            p.WriteByte((byte)elements);

            for (int i = 0; i < elements; i++)
            {
                p.WriteBytes(v_element[i].Build());
            }
            return p.GetBytes;
        }
        public static byte[] pacote11F(PlayerInfo pi, short tipo)
        {
            var p = new PangyaBinaryWriter();
            if (pi == null)
                throw new Exception("Erro PlayerInfo *pi is nullptr. packet_func::pacote11F()");

            p.init_plain(0x11F);

            p.WriteInt16(tipo);

            p.WriteStruct(pi.TutoInfo, new TutorialInfo());
            return p.GetBytes;
        }

        public static byte[] pacote1A9(int ttl_milliseconds/*time to live*/, int option = 0)
        {
            var p = new PangyaBinaryWriter();
            p.init_plain(0x1A9);

            p.WriteByte((byte)option);

            p.WriteInt32(ttl_milliseconds);
            return p.GetBytes;
        }

        public static byte[] pacote095(short sub_tipo, int option = 0, PlayerInfo pi = null)
        {
            var p = new PangyaBinaryWriter();
            p.init_plain(0x95);

            p.WriteInt16(sub_tipo);

            if (sub_tipo == 0x102)
                p.WriteByte((byte)option);

            else if (sub_tipo == 0x111)
            {
                p.WriteInt32(option);

                if (pi == null)
                {
                    //delete p;

                    throw new Exception("Erro PlayerInfo *pi is nullptr. packet_func::pacote095()");
                }

                p.WriteUInt64(pi.ui.pang);
            }
            return p.GetBytes;
        }

        public static byte[] pacote25D(List<TrofelEspecialInfo> v_tgp_current_season, int v)
        {
            // throw new NotImplementedException();
            return null;
        }
        public static byte[] pacote156(uint _uid, UserEquip _ue, byte season)
        {
            var p = new PangyaBinaryWriter();
            p.init_plain(0x156);

            p.WriteByte(season);

            p.WriteUInt32(_uid);             
            p.WriteBytes(_ue.Build());     
            return p.GetBytes;
        }


        public static byte[] pacote157(MemberInfoEx _mi, byte season)
        {
            var p = new PangyaBinaryWriter();
            p.init_plain(0x157);

            p.WriteByte(season);

            p.WriteUInt32(_mi.uid);
            p.WriteUInt16(_mi.sala_numero);
            p.WriteBytes(_mi.Build());
            p.WriteUInt32(_mi.uid);
            p.WriteUInt32(_mi.guild_point);
             return p.GetBytes;
        }

        public static byte[] pacote158(uint _uid, UserInfoEx _ui, byte season)
        {
            var p = new PangyaBinaryWriter();
            p.init_plain(0x158);

            p.WriteByte((byte)season);

            p.WriteUInt32(_uid);

            p.WriteBytes(_ui.Build());                                     
            return p.GetBytes;
        }

        public static byte[] pacote159(uint uid, TrofelInfo ti, byte season)
        {
            var p = new PangyaBinaryWriter(0x159);
            p.WriteByte(season);
            p.WriteUInt32(uid);
            p.WriteBytes(ti.Build());
            return p.GetBytes;
        }

        public static byte[] pacote15A(uint uid, List<TrofelEspecialInfo> vTei, byte season)
        {
            var p = new PangyaBinaryWriter(0x15A);
            p.WriteByte(season);
            p.WriteUInt32(uid);
            p.WriteUInt16((ushort)vTei.Count);

            foreach (var item in vTei)
                p.WriteStruct(item, new TrofelEspecialInfo());

            return p.GetBytes;
        }

        public static byte[] pacote15B(uint uid, byte season)
        {
            var p = new PangyaBinaryWriter(0x15B);
            p.WriteByte(season);
            p.WriteUInt32(uid);
            p.WriteInt16(0); // Count desconhecido
            return p.GetBytes;
        }

        public static byte[] pacote15C(uint uid, List<MapStatisticsEx> vMs, List<MapStatisticsEx> vMsa, byte season)
        {
            var p = new PangyaBinaryWriter(0x15C);
            p.WriteByte(season);
            p.WriteUInt32(uid);
            p.WriteInt32(vMs.Count);

            foreach (var item in vMs)
                p.WriteBytes(item.Build());

            p.WriteInt32(vMsa.Count);

            foreach (var item in vMsa)
                p.WriteBytes(item.Build());

            return p.GetBytes;
        }

        public static byte[] pacote15D(uint uid, GuildInfo gi)
        {
            var p = new PangyaBinaryWriter(0x15D);
            p.WriteUInt32(uid);
            p.WriteBytes(gi.Build());
            return p.GetBytes;
        }

        public static byte[] pacote15E(uint uid, CharacterInfo ci)
        {
            var p = new PangyaBinaryWriter(0x15E);
            p.WriteUInt32(uid);
            p.WriteBytes(ci.Build());
            return p.GetBytes;
        }

        public static byte[] pacote096(PlayerInfo pi)
        {
            if (pi == null)
                throw new Exception("Erro PlayerInfo *pi is nullptr. packet_func::pacote096()");
            using (var p = new PangyaBinaryWriter(0x96))
            {                          
                p.WriteUInt64(pi.cookie);
                return p.GetBytes;
            }     
        }

        public static byte[] pacote181(List<ItemBuffEx> v_element, int option = 0)
        {
            using (var p = new PangyaBinaryWriter(0x181))
            {                                             
                p.WriteInt32(option);

                if (option == 0)
                {
                    p.WriteByte(v_element.Count());
                    for (int i = 0; i < v_element.Count; i++)
                        p.WriteBytes(v_element[i].Build());

                }
                else if (option == 2)
                {
                    p.WriteUInt32((uint)v_element.Count);

                    for (int i = 0; i < v_element.Count; i++)
                    {
                        p.WriteUInt32(v_element[i]._typeid);
                        p.WriteBytes(v_element[i].Build());

                    }
                }
                else
                    p.WriteByte(0);

                return p.GetBytes;
            }
        }

        public static byte[] pacote13F()
        {
            // throw new NotImplementedException();
            return null;
        }


        public static byte[] pacote136()
        {
            // throw new NotImplementedException();
            return null;
        }

        public static byte[] pacote137(CardEquipManager v_element)
        {
            using (var p = new PangyaBinaryWriter())
            {
                p.init_plain(0x137);

                p.WriteUInt16((short)v_element.Count());
                foreach (var CardEquip in v_element.Values)
                    p.WriteBytes(CardEquip.Build());

                return p.GetBytes;
            }
        }
        public static byte[] pacote138(CardManager v_element, int option = 0)
        {
            using (var p = new PangyaBinaryWriter())
            {
                p.Write(new byte[] { 0x38, 0x01 });
                p.WriteInt32(option);
                p.WriteUInt16((ushort)v_element.Count);
                foreach (var Card in v_element.Values)
                    p.WriteBytes(Card.Build());

                return p.GetBytes;
            }
        }

        public static byte[] pacote135()
        {
            // throw new NotImplementedException();
            return null;
        }

        public static byte[] pacote131(ref packet p)
        {
            // throw new NotImplementedException();
            return null;
        }

        public static byte[] pacote072(UserEquip ue)
        {
            var p = new PangyaBinaryWriter();

            p.Write(new byte[] { 0x72, 0x00 });
            p.WriteBytes(ue.Build());
            return p.GetBytes;
        }

        public static byte[] pacote0E1(MascotManager v_element, int option = 0)
        {
            var p = new PangyaBinaryWriter();

            p.init_plain(0xE1);
            p.Write(v_element.Build());
            return p.GetBytes;
        }                
        public static PangyaBinaryWriter pacote073(List<WarehouseItemEx> v_element, int option = 0)
        {
            var p = new PangyaBinaryWriter();
            try
            {
                p.Write(new byte[] { 0x73, 0x00 });
                p.WriteUInt16((short)v_element.Count);
                p.WriteUInt16((short)v_element.Count);
                foreach (var item in v_element)
                {
                    p.WriteBytes(item.Build());
                }
                return p;
            }
            catch (Exception)
            {
                return p;
            }
        }

        public static byte[] pacote071(CaddieManager v_element, int option = 0)
        {
            var p = new PangyaBinaryWriter();
            try
            {
                p.Write(new byte[] { 0x71, 0x00 });
                p.WriteInt16((short)v_element.Count);
                p.WriteInt16((short)v_element.Count);
                foreach (var char_info in v_element.Values)
                {
                    p.WriteBytes(char_info.Build(false));
                }
                return p.GetBytes;
            }
            catch (Exception)
            {
                return p.GetBytes;
            }
        }

        /// <summary>
        /// Send Packet for Info Characters(Personagens)
        /// </summary>
        /// <param name="v_element">object list</param>
        /// <param name="option">what?</param>
        /// <returns>obj using for write data</returns>
        public static byte[] pacote070(CharacterManager v_element, int option = 0)
        {
            var p = new PangyaBinaryWriter();
            try
            {
                p.Write(new byte[] { 0x70, 0x00 });
                p.WriteInt16((short)v_element.Count);
                p.WriteInt16((short)v_element.Count);
                foreach (var char_info in v_element.Values)
                {
                    p.WriteBytes(char_info.Build());
                }
                return p.GetBytes;
            }
            catch (Exception)
            {
                return p.GetBytes;
            }
        }

        /// <summary>
        /// packet 9D use channel list!
        /// </summary>
        /// <param name="v_element"></param>
        /// <param name="build_s">true is server, false is chanell call!</param>
        /// <returns></returns>
        public static byte[] pacote04D(List<Channel> v_element, bool build_s = false)
        {
            try
            {
                using (var p = new PangyaBinaryWriter())
                {
                    if (!build_s)
                        p.Write(new byte[] { 0x4D, 0x00 }); //channel list!         

                    p.WriteByte(v_element.Count);
                    foreach (var channel in v_element)
                        p.WriteBytes(channel.Build());

                    return p.GetBytes;
                }
            }
            catch (Exception ex)
            {
                _smp.message_pool.push(new message(
              $"[packet_func::pacote04D][ErrorSystem] {ex.Message}\nStack Trace: {ex.StackTrace}",
              type_msg.CL_FILE_LOG_AND_CONSOLE));
                return null;
            }
        }

        public static byte[] pacote248(
            AttendanceRewardInfo ari,
            int option = 0)
        {
            using (var p = new PangyaBinaryWriter())
            {
                p.Write(new byte[] { 0x48, 0x02 });
                p.WriteInt32(option);
                p.WriteBytes(ari.Build());
                return p.GetBytes;
            }
        }

        public static byte[] pacote249(
            AttendanceRewardInfo ari,
            int option = 0)
        {
            using (var p = new PangyaBinaryWriter())
            {
                p.Write(new byte[] { 0x49, 0x02 });
                p.WriteInt32(option);
                p.WriteBytes(ari.Build());
                return p.GetBytes;
            }
        }

        public static byte[] pacote257(uint _uid, List<TrofelEspecialInfo> v_tegi, byte season)
        {
            using (var p = new PangyaBinaryWriter())
            {
                p.init_plain(0x257);

                p.WriteByte(season);
                p.WriteUInt32(_uid);

                p.WriteInt16((short)v_tegi.Count);
                foreach (var item in v_tegi)
                    p.WriteStruct(item, new TrofelEspecialInfo()); 
                    return p.GetBytes;
            }
        }
       
        public static byte[] pacote04E(int option, int _codeErrorInfo = 0)
        {
            /* Option Values
                * 1 Sucesso
                * 2 Channel Full
                * 3 Nao encontrou canal
                * 4 Nao conseguiu pegar informções do canal
                * 6 ErrorCode Info
                */
            var p = new PangyaBinaryWriter();
            p.init_plain(0x4E);

            p.WriteByte((byte)option);

            if (_codeErrorInfo != 0)
                p.WriteInt32(_codeErrorInfo);
            return p.GetBytes;
        }


       public static byte[] pacote040(PlayerInfo pi, string msg, byte option)
        {

            if ((option == 0 || option == 0x80) && pi == null)
                throw  new exception("Error PlayerInfo *pi is nullptr. packet_func::pacote040()");

            using (var p = new PangyaBinaryWriter(0x40))
            {                                          
                p.WriteByte(option);

                if (option == 0 || option == 0x80)
                {
                    p.WritePStr(pi.nickname);
                    p.WritePStr(msg);
                }

                return p.GetBytes;
            }
        }

        public static byte[] pacote044(ServerInfoEx _si, int option, PlayerInfo pi = null, int valor = 0)
        {
            var p = new PangyaBinaryWriter(0x44);

            if (option == 0 && pi == null)
                throw new Exception("Erro PlayerInfo *pi is nullptr. packet_func::pacote044()");
                                            
            p.WriteByte(option);   // Option

            if (option == 0)
                p.Write(InitialLogin(pi, _si));
            else if (option == 1)
                p.WriteByte(0);
            else if (option == 0xD3)
                p.WriteByte(0);
            else if (option == 0xD2)
                p.WriteInt32(valor);

            return p.GetBytes;
        }

        public static byte[] pacote0B2(

List<MsgOffInfo> v_element,
int option = 0)
        {
            var p = new PangyaBinaryWriter();

            p.init_plain((ushort)0xB2);

            p.WriteInt32(2); // Não sei bem o que é, mas pode ser uma opção

            p.WriteInt32(option);

            p.WriteUInt32((uint)v_element.Count);

            foreach (MsgOffInfo i in v_element)
            {
                p.WriteStruct(i, new MsgOffInfo());
            }

            return p.GetBytes;
        }
        public static byte[] pacote0D4(CaddieManager v_element)
        {
            using (var p = new PangyaBinaryWriter())
            {
                p.init_plain(0xD4);
                p.WriteUInt32((uint)v_element.Count());
                foreach (var item in v_element.Values)
                    p.WriteBytes(item.Build());

                return p.GetBytes;
            }
        }

        // Metôdos de auxílio de criação de pacotes


        public static byte[] pacote210(

                List<MailBox> v_element,
                int option = 0)
        {
            var p = new PangyaBinaryWriter();

            p.init_plain((ushort)0x210);

            p.WriteInt32(option);

            p.WriteInt32(v_element.Count);

            for (var i = 0; i < v_element.Count; ++i)
            {
                p.WriteBytes(v_element[i].Build());
            }

            return p.GetBytes;
        }

        internal static byte[] pacote101(int option = 0)
        {
            var p = new PangyaBinaryWriter();

            p.init_plain((ushort)0x101);

            p.WriteByte((byte)option);
            return p.GetBytes;
        }
        public static byte[] pacote0B4(

List<TrofelEspecialInfo> v_element,
int option = 0)
        {
            var p = new PangyaBinaryWriter();

            p.init_plain((ushort)0xB4);

            p.WriteInt16((short)option);

            p.WriteByte((byte)v_element.Count);

            foreach (TrofelEspecialInfo i in v_element)
            {
                p.WriteStruct(i, new TrofelEspecialInfo());
            }

            return p.GetBytes;
        }

        public static byte[] pacote0F1(

int option = 0)
        {
            var p = new PangyaBinaryWriter();

            p.init_plain((ushort)0xF1);

            p.WriteByte((byte)option);

            return p.GetBytes;
        }


        public static byte[] pacote169(
           TrofelInfo ti,
            int option = 0)
        {
            var p = new PangyaBinaryWriter();
            p.init_plain((ushort)0x169);

            p.WriteByte((byte)option);

            p.WriteBytes(ti.Build());

            return p.GetBytes;
        }

        public static byte[] pacote09F(List<ServerInfo> v_server, List<Channel> v_channel)
        {
            using (var p = new PangyaBinaryWriter((ushort)0x9F))
            {
                p.WriteByte((byte)v_server.Count);

                for (var i = 0; i < v_server.Count; ++i)
                {
                    p.WriteBytes(v_server[i].Build());
                }
                p.WriteBytes(pacote04D(v_channel, true));
                return p.GetBytes;
            }
        }

        public static byte[] pacote089(uint _uid = 0, byte season = 0, uint err_code = 1)
        {

            using (var p = new PangyaBinaryWriter((ushort)0x89))
            {
                p.WriteUInt32(err_code);
                if (err_code > 0)
                {
                    p.WriteByte(season);
                    p.WriteUInt32(_uid);
                }
                return p.GetBytes;                      
            }
        }                                                                     

        public static byte[] pacote211(List<MailBox> v_element, uint pagina, uint paginas, uint error = 0)
        {

            using (var p = new PangyaBinaryWriter(0x211))
            {
                p.WriteUInt32(error);

                if (error == 0)
                {
                    p.WriteUInt32(pagina);
                    p.WriteUInt32(paginas);
                    p.WriteInt32(v_element.Count);

                    for (int i = 0; i < v_element.Count; ++i)
                    {
                        p.WriteBytes(v_element[i].Build());
                    }
                }

                return p.GetBytes;
            }
        }

        public static byte[] pacote212(EmailInfo ei, uint error = 0)
        {

            using (var p = new PangyaBinaryWriter(0x212))
            {
                p.WriteUInt32(error);

                if (error == 0)
                {
                    p.WriteBytes(ei.Build());  
                }

                return p.GetBytes;
            }
        }


        public static byte[] pacote06B(PlayerInfo pi, byte type, byte err_code = 4)
        {

            if (pi == null)
            {
                throw new exception("Erro PlayerInfo *pi is nullptr. packet_func::pacote06B()", ExceptionError.STDA_MAKE_ERROR_TYPE(STDA_ERROR_TYPE.PACKET_FUNC_SV,
                    1, 0));
            }
            var p = new PangyaBinaryWriter(0x6B);

            p.WriteByte(err_code); // Error Code, 4 Sucesso, diferente é erro
            p.WriteByte(type);

            if (err_code == 4)
            {
                switch (type)
                {
                    case 0: // Character Equipado Com os Parts Equipado
                        if (pi.ei.char_info != null)
                        {
                            p.WriteBytes(pi.ei.char_info.Build());//, sizeof(CharacterInfo));
                        }
                        else
                        {
                            p.WriteZero(513);
                        }
                        break;
                    case 1: // Caddie Equipado
                        if (pi.ei.cad_info != null)
                        {
                            p.WriteUInt32(pi.ei.cad_info.id);
                        }
                        else
                        {
                            p.WriteZero(4);
                        }
                        break;
                    case 2: // Itens Equipáveis
                        p.WriteUInt32(pi.ue.item_slot);//, sizeof(pi.ue.item_slot));
                        break;
                    case 3: // Ball e Clubset Equipado
                        if (pi.ei.comet != null) // Ball
                        {
                            p.WriteUInt32(pi.ei.comet._typeid);
                        }
                        else
                        {
                            p.WriteZero(4);
                        }
                        p.WriteUInt32(pi.ei.csi.id); // ClubSet ID
                        break;
                    case 4: // Skins
                        p.WriteUInt32(pi.ue.skin_typeid);//, sizeof(pi.ue.skin_typeid));
                        break;
                    case 5: // Only Chracter Equipado
                        if (pi.ei.char_info != null)
                        {
                            p.WriteUInt32(pi.ei.char_info.id);
                        }
                        else
                        {
                            p.WriteZero(4);
                        }
                        break;
                    case 8: // Mascot Equipado
                        if (pi.ei.mascot_info != null)
                        {
                            p.WriteBytes(pi.ei.mascot_info.Build());//, sizeof(MascotInfo));
                        }
                        else
                        {
                            p.WriteZero(62);
                        }
                        break;
                    case 9: // Character Cutin Equipado
                        if (pi.ei.char_info != null)
                        {
                            p.WriteUInt32(pi.ei.char_info.id);
                            p.WriteUInt32(pi.ei.char_info.Cut_in);//, sizeof(pi.ei.char_info.cut_in));
                        }
                        else
                        {
                            p.WriteZero(20);
                        }
                        break;
                    case 10: // Poster Equipado
                        p.WriteUInt32(pi.ue.poster);//, sizeof(pi.ue.poster));
                        break;
                }
            }

            return p.GetBytes;
        }
        #endregion

    }
}
