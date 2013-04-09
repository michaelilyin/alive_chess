/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package commands.Util;

/**
 *
 * @author Admin
 */
public class Commands {
    
    // команды авторизации:   диапазон: 0 - 5 количество: (6)
    public static final int AUTHORISE_REQUEST = 0;
    public static final int AUTHORISE_RESPONSE = 1;
    public static final int EXIT_FROM_GAME_REQUEST = 2;
    public static final int EXIT_FROM_GAME_RESPONSE = 3;
    public static final int REGISTER_REQUEST = 4;
    public static final int REGISTER_RESPONSE = 5;

    // команды большой карты: диапазон: 6 - 55 количество: (50)
    public static final int BIG_MAP_REQUEST = 6;
    public static final int BIG_MAP_RESPONSE = 7;
    public static final int CAPTURE_CASTLE_REQUEST = 8;
    public static final int CAPTURE_CASTLE_RESPONSE = 9;
    public static final int CAPTURE_MINE_REQUEST = 10;
    public static final int CAPTURE_MINE_RESPONSE = 11;
    public static final int COME_IN_CASTLE_REQUEST = 12;
    public static final int COME_IN_CASTLE_RESPONSE = 13;
    public static final int CONTACT_KING_REQUEST = 14;
    public static final int CONTACT_KING_RESPONSE = 15;
    public static final int TAKE_RESOURCE_MESSAGE = 16;
    public static final int MOVE_KING_REQUEST = 17;
    public static final int MOVE_KING_RESPONSE = 18;
    public static final int CONTACT_CASTLE_REQUEST = 19;
    public static final int CONTACT_CASTLE_RESPONSE = 20;
    public static final int GET_MAP_REQUEST = 21;
    public static final int GET_MAP_RESPONSE = 22;
    public static final int GET_OBJECTS_REQUEST = 23;
    public static final int GET_OBJECTS_RESPONSE = 24;
    public static final int LOOSE_CASTLE_MESSAGE = 25;
    public static final int LOOSE_MINE_MESSAGE = 26;
    public static final int UPDATE_WORLD_MESSAGE = 27;
    public static final int GET_RESOURCE_MESSAGE = 28;
    public static final int GET_UNITY_MAP_REQUEST = 29;
    public static final int GET_UNITY_MAP_RESPONSE = 30;
    public static final int GET_GAME_STATE_REQUEST = 31;
    public static final int GET_GAME_STATE_RESPONSE = 32;
    public static final int COMPUTE_PATH_REQUEST = 33;
    public static final int COMPUTE_PATH_RESPONSE = 34;
    public static final int VERIFY_PATH_REQUEST = 35;
    public static final int VERIFY_PATH_RESPONSE = 36;
    public static final int GET_KING_REQUEST = 37;
    public static final int GET_KING_RESPONSE = 38;

    // команды системного диалога: диапазон: 56 - 75 количество: (20)
    public static final int BATTLE_DIALOG_MESSAGE = 56;
    public static final int PAY_OFF_DIALOG_MESSAGE = 57;
    public static final int MARKET_DIALOG_MESSAGE = 59;
    public static final int CAPITULATE_DIALOG_MESSAGE = 60;
    public static final int CAPTURE_CASTLE_DIALOG_MESSAGE = 61;
    public static final int DEACTIVATE_DIALOG_MESSAGE = 62;
    public static final int CREATE_UNION_DIALOG_MESSAGE = 63;
    public static final int WAR_DIALOG_MESSAGE = 64;
    public static final int PEACE_DIALOG_MESSAGE = 65;
    public static final int JOIN_EMPERIES_DIALOG_MESSAGE = 66;

    // команды замка: диапазон: 76 - 125(50)
    public static final int LEAVE_CASTLE_REQUEST = 76;
    public static final int LEAVE_CASTLE_RESPONSE = 77;
    public static final int GET_ARMY_CASTLE_TO_KING_REQUEST = 78;
    public static final int GET_ARMY_CASTLE_TO_KING_RESPONSE = 79;
    public static final int GET_LIST_BUILDINGS_IN_CASTLE_REQUEST = 80;
    public static final int GET_LIST_BUILDINGS_IN_CASTLE_RESPONSE = 81;
    public static final int GET_REC_BUILDINGS_REQUEST = 82;
    public static final int GET_REC_BUILDINGS_RESPONSE = 83;
    public static final int SHOW_ARMY_CASTLE_REQUEST = 84;
    public static final int SHOW_ARMY_KING_REQUEST = 85;
    public static final int BUILDING_IN_CASTLE_REQUEST = 86;
    public static final int BUILDING_IN_CASTLE_RESPONSE = 87;
    public static final int BUY_FIGURE_REQUEST = 88;
    public static final int BUY_FIGURE_RESPONSE = 89;

    // команды шахматного поединка: диапазон 126 - 145(20)
    public static final int BATTLE_MESSAGE = 127;
    public static final int DOWNLOAD_BATTLEFIELD_REQUEST = 128;
    public static final int DOWNLOAD_BATTLEFIELD_RESPONSE = 129;
    public static final int MOVE_UNIT_REQUEST = 130;
    public static final int MOVE_UNIT_RESPONSE = 131;

    // команды управлениями союзами и империями: 146 - 205 (50)
    public static final int JOIN_REQUEST_MESSAGE = 149;
    public static final int EMBED_TAX_RATE_REQUEST = 150;
    public static final int EMBED_TAX_RATE_RESPONSE = 151;
    public static final int EXCLUDE_KING_FROM_EMPIRE_REQUEST = 152;
    public static final int EXCLUDE_KING_FROM_EMPIRE_RESPONSE = 153;
    public static final int INCLUDE_KING_IN_EMPIRE_REQUEST = 154;
    public static final int INCLUDE_KING_IN_EMPIRE_RESPONSE = 155;
    public static final int EXIT_FROM_ALIANCE_REQUEST = 156;
    public static final int EXIT_FROM_ALIANCE_RESPONSE = 157;
    public static final int GET_HELP_FIGURE_REQUEST = 158;
    public static final int GET_HELP_FIGURE_RESPONSE = 159;
    public static final int GET_HELP_RESOURCE_REQUEST = 160;
    public static final int GET_HELP_RESOURCE_RESPONSE = 161;
    public static final int GET_ALIANCE_INFO_REQUEST = 162;
    public static final int GET_ALIANCE_INFO_RESPONSE = 163;
    public static final int GRAND_LEADER_PRIVILEGES_MESSAGE = 164;
    public static final int TAKE_AWAY_LEADER_PRIVILEGES_MESSAGE = 165;
    public static final int JOIN_TO_ALIANCE_REQUEST = 166;
    public static final int JOIN_TO_ALIANCE_RESPONSE = 167;
    public static final int MESSAGE_NEWS_MESSAGE = 168;
    public static final int SEND_FIGURE_HELP_MESSAGE = 169;
    public static final int SEND_RESOURCE_HELP_MESSAGE = 170;
    public static final int START_VOTE_REQUEST = 171;
    public static final int START_VOTE_RESPONSE = 172;
    public static final int START_IMPEACHMENT_REQUEST = 173;
    public static final int START_IMPEACHMENT_RESPONSE = 174;
    public static final int VOTE_BALLOT_MESSAGE = 175;
    public static final int GET_ALIANCES_INFO_REQUEST = 176;
    public static final int GET_ALIANCES_INFO_RESPONSE = 177;
    public static final int EXCLUDE_FROM_EMPIRE_MESSAGE = 178;
    public static final int START_NEGOTIATE_REQUEST = 179;
    public static final int START_NEGOTIATE_RESPONSE = 180;

    // команды торговли:           ?

    // команды статистики: 206 - 225(20)
    public static final int GET_STATISTIC_REQUEST = 206;
    public static final int GET_STATISTIC_RESPONSE = 207;
    public static final int GET_AVAILABLE_MAPS_REQUEST = 208;
    public static final int GET_AVAILABLE_MAPS_RESPONSE = 209;


        // --------------------- Сектор команд чата (250 - 280)----------------------------------------- //

        // команды чата
    public static final int JOIN_LEAVE_COMMAND = 250;
    public static final int MESSAGE_COMMAND = 251;
    public static final int MESSAGE_RECEIVE_COMMAND = 252;
    public static final int SEND_CONTACT_LIST_COMMAND = 253;

    public static final int ERROR_MESSAGE = 666;
}
