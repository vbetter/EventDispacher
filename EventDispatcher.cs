using System;
using System.Collections.Generic;
using WLGame;

namespace WLGame
{
    public enum EnmEvn
    {
        EVN_ADD_HERO,            //增加英雄事件, data : dgame.Hero
        EVN_REMOVE_HERO,         //删除英雄事件, data : dgame.Hero
        EVN_HERO_REBORN,         // 英雄重生

        EVN_ACTORBEHAVIOUR_DEAD,  //actor死亡, data : ActorBehaviour

        EVN_BAG_ITEMS_UPDATED,  // 背包内的物品发生了变更, data : null
        EVN_MODIFY_GOLD,           //增加金币, data : uint
        EVN_MODIFY_DIAMOND,         //增加钻石, data : uint
        EVN_MODIFY_STEMINA,        //增加体力, data : uint
        EVN_MODIFY_VIPLEVEL,       //VIP等级改变, data : uint
        EVN_MODIFY_LEVEL,          //等级改变
        EVN_MODIFY_EXP,            //经验改变
        EVN_MODIFY_HONOR,               //荣誉值改变
        EVN_MODIFY_EXPLOIT,             //功勋值改变
        EVN_MODIFY_MAGIC_STONE,         //灵石数目改变
        EVN_MODIFY_SOUL_JADE,           //魂玉数目改变

        EVN_HERO_RISE_ENABLE, //允许英雄升阶
        
        EVN_FIRST_MEET_ENEMY,            //第一次遇见敌人, data : ActorBehaviour (view's owner ActorBehaviour)

        EVN_PAY_SUCCESSFUL,            //充值成功

        EVN_TASK_HAVE_DONE,            //做好任务，
        EVN_SIGNIN_HAVE_DONE,          //签到完成，
        EVN_MAIL_HAVE_DONE,            //邮件收取完成，

        EVN_PLAYSKILL,             //统计战斗中技能释放

        EVN_RECEIVE_REWARD_BOX,  //已领取消息，更新奖励宝箱的状态，目前只在选关和任务界面使用

        EVN_HERO_UPDATE_ITEM,//英雄召唤后通知界面刷新item
        //新手引导
        EVN_NEW_GUIDE_TRIGGER,//新手引导触发

        //Buff触发事件
        EVN_BUFF_TRIGER_SKILL_DAMAGE, //buff加持，受到技能攻击时触发buff事件
        EVN_BUFF_TRIGER_NOMARL_DAMAGE, //buff加持，受到普通攻击时触发buff事件
        EVN_BUFF_TRIGER_BUFF_DAMAGE, //buff加持，受到buff伤害时触发buff事件
        EVN_BUFF_TRIGER_HP_PRECENTAGE, //buff加持，血量降低到百分比时触发BUFF

        //本地战斗
        EVN_LOC_BATTLE_START,           //战斗模拟开始
        EVN_LOC_BATTLE_END,             //战斗模拟结束
        //统计
        EVN_RECORD_INIT,//统计员初始化
        EVN_RECORD_BATTLE_START,//统计开始
        EVN_RECORD_BATTLE_RESULT, //统计战斗结果
        EVN_RECORD_ACTOR_DIE,//角色死亡

        //战场触发器
        EVN_TRIGGER_PRELOAD_HERO,//预加载英雄，特殊需求，只处理体验关
        EVN_TRIGGER_CREATE_HERO,//创建英雄
        EVN_TRIGGER_CREATE_MONSTER,//创建怪物
        EVN_TRIGGER_CAMERA_CTRL,//摄像机控制
        EVN_TRIGGER_BOSS_DIE,//boss死亡
        EVN_TRIGGER_FIGHT_WIN,//战斗胜利
        EVN_TRIGGER_FIGHT_FIAL,//战斗失败
        EVN_TRIGGER_PAUSE_BATTLE,//暂停/恢复战场
        EVN_TRIGGER_CLEAR_BATTLE,//清理战场

        //小红点
        EVN_TRIGGER_ALL_RED_POINT,      //场景小红点
        EVN_TRIGGER_TOOL_RED_POINT,      //小红点
        EVN_TRIGGER_PIECE_RED_POINT,      //小红点
        EVN_TRIGGER_OPEN_ACTIVE_RED_POINT = 49,//开服活动红点（lua也用到 对应49，不要改变其值）

        EVN_CLOSE_ACTIVE_PANEL = 50,//关闭活动界面（lua也用到 对应50，不要改变其值）

        EVN_UI_EVENT_DESTORY ,//ui3wnd关闭事件
        EVN_UI_EVENT_CHAT_RED_POINT,//主界面聊天按钮上的小红点
        //buff测试面板
        EVN_BUFF_TEST_CREATE,//buff创建
        EVN_BUFF_TEST_DISTROY,//buff销毁
        EVN_UPDATE_DESTINY_LEVEL,//更新天命

        EVN_MODIFY_INTEGRAL,           //积分数目改变
        EVN_REFRESH_FAMILY_SKILL,      //刷新家族技能数据

        EVN_TRIGGER_SEVEN_DAYS_ACTIVITY_RED_POINT = 58,//七日狂欢红点（lua也用到，不要改变其值）

        EVN_MAX                      //无实际意义，事件的最大值，用于new数组

    }

    /*
     * simple example
     *
           //define delegate function
            public void OnAddHero(EnmEvn evn, System.Object data);
     
            //regist care event and delegate function
            dispatcher.Regist(EnmEvn.EVN_ADD_HERO, OnAddHero);
            
            //dispatch event
            dgame.Hero hero = new dgame.Hero();
            dispatcher.Dispatch(EnmEvn.EVN_ADD_HERO, hero);
     */

    public class EventDispatcher : LogicSystem
    {
        public delegate void DCEvnHandle(EnmEvn evn, System.Object data);

        class EventEntry
        {
            // 事件
            EnmEvn _evn;

            // 事件的观察者列表
            List<DCEvnHandle> _observers = new List<DCEvnHandle>();

            public EventEntry(EnmEvn evn)
            {
                _evn = evn;
            }

            public void Add(DCEvnHandle observer)
            {
                _observers.Add(observer);
            }

            public void Remove(DCEvnHandle observer)
            {
                _observers.Remove(observer);
            }

            public void RemoveByTarget(Object target)
            {
                if (target != null)
                {
                    for (int i = 0; i < _observers.Count;)
                    {
                        if (_observers[i] != null && _observers[i].Target == target)
                        {
                            _observers.RemoveAt(i);
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
            }

            public void Call(EnmEvn evn, System.Object data)
            {
                for (int i = 0; i < _observers.Count; i++)
                {
                    if (_observers[i] != null)
                    {
                        _observers[i](evn, data);
                    }
                }
            }
        }

        Dictionary<EnmEvn, EventEntry> _evnEntries = new Dictionary<EnmEvn, EventEntry>();

        private static EventDispatcher sInstance = null;
        public static EventDispatcher Instance
        {
            get
            {
                if (sInstance == null)
                {
                    sInstance = new EventDispatcher();
                }
                return sInstance;
            }
        }

        //分发事件
        public void Dispatch(EnmEvn evn, System.Object data)
        {
            EventEntry entry = GetEventEntry(evn);
            if (entry != null)
            {
                entry.Call(evn, data);
            }
        }

        //注册关注的事件
        public void Regist(EnmEvn evn, DCEvnHandle handle)
        {
            if (evn >= EnmEvn.EVN_MAX)
            {
                return;
            }

            EventEntry entry = GetEventEntry(evn);
            if (entry == null)
            {
                entry = AddEventEntry(evn);
            }

            entry.Add(handle);
        }

        //取消关注的事件
        public void UnRegist(EnmEvn evn, DCEvnHandle handle)
        {
            if (evn >= EnmEvn.EVN_MAX)
            {
                return;
            }

            EventEntry entry = GetEventEntry(evn);
            if (entry != null)
            {
                entry.Remove(handle);
            }
        }

        //清除所有事件
        public void CleanAllEvn()
        {
            _evnEntries.Clear();
        }

        /// <summary>
        /// 移除所有由对象target所注册的事件监听器。
        /// 通常来说，可在事件监听器所属对象被销毁之前调用该方法。
        /// </summary>
        public void RemoveAllListenersOfTarget(Object target)
        {
            if (target == null)
            {
                return;
            }

            foreach (var item in _evnEntries)
            {
                EventEntry entry = item.Value;
                if (entry != null)
                {
                    entry.RemoveByTarget(target);
                }
            }
        }

        EventEntry GetEventEntry(EnmEvn evn)
        {
            EventEntry entry = null;
            _evnEntries.TryGetValue(evn, out entry);
            return entry;
        }

        EventEntry AddEventEntry(EnmEvn evn)
        {
            EventEntry entry = new EventEntry(evn);
            _evnEntries[evn] = entry;

            return entry;
        }
    }
}
