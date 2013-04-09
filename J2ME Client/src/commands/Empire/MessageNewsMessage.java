/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Empire;

import commands.Util.Commands;
import commands.Util.ICommand;
import Logic.Contexts.CommandListeners.IEmpireCommandListener;
import Serializer.Utils.ComputeSizeUtil;
import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.FieldSerializer;
import Serializer.Utils.IProtoDeserializable;
import Serializer.Utils.IProtoSerializableRequest;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class MessageNewsMessage implements IProtoSerializableRequest, IProtoDeserializable, ICommand {
    private int com_id;
    private int news;           //proto 1
        //No                              = 0,
        //HelpFigure                      = 1,  // игрок запросил фигуры
        //HelpResource                    = 2,  // игрок запросил ресурсы
        //PlayerWantJoinToAliance         = 3,  //
        //PlayerJoinedToAliance           = 4,  // игрок присоединился к союзу
        //PlayerLeaveAliance              = 5,  // игрок вышел из союза
        //PlayerExcludedFromEmpire        = 6,  // игрок выгнан из союза
        //HelpFigureSended                = 7,  //
        //HelpResourceSended              = 8,  //
        //VoteStarted                     = 9,  // начало голосования
        //VoteEndedResultPublished        = 10, // окончание голосования и объявление результатов
        //ImpeachmentStarted              = 11, // начало импичмента
        //ImpeachmentEndedResultPublished = 12, // окончание импичмента и объявление результатов
        //LeaderEnterInGame               = 13, //
        //LeaderExitFromGame              = 14, //
        //PlayerEnterInGame               = 15, //
        //PlayerExitFromGame              = 16, //
        //NewTaxEmbeded                   = 17, // установлен новый налог
        //ChangeAlianceStatus             = 18, // союз превратился в империю либо наоборот
    private String message;     //proto 2
    private int senderId;       //proto 3
    public MessageNewsMessage(){com_id = Commands.MESSAGE_NEWS_MESSAGE;}

    public int getNews(){return news;}
    public void setNews(int value){news = value;}
    public String getMessage(){return message;}
    public void setMessage(String value){message = value;}
    public int getSenderId(){return senderId;}
    public void setSenderId(int value){senderId = value;}

    public int ComputeSize() {
        return ComputeSizeUtil.ComputeInt(1, news) +
               ComputeSizeUtil.ComputeString(2, message) +
               ComputeSizeUtil.ComputeInt(3, senderId);
    }

    public byte[] toByte() throws IOException {
        int len = ComputeSize();
        byte[] result = new byte[len+8];
        FieldSerializer sr = new FieldSerializer(result);
        sr.WriteIntNonSerialized(com_id);
        sr.WriteIntNonSerialized(len);
        sr.SerializeInt(1, news);
        sr.SerializeString(2, message);
        sr.SerializeInt(3, senderId);
        return result;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        news = dsr.readInt(1);
        message = dsr.readString(2);
        senderId = dsr.readInt(3);
    }

    public void Execute(Object listener) {
        IEmpireCommandListener l = (IEmpireCommandListener)listener;
        l.MessageNewsMessReceived(this);
    }


}
