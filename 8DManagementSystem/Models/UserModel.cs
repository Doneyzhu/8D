using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _8DManagementSystem.Models
{
    public class UserModel
    {

        public UserModel()
        {
            Board_User_Role_Models = new List<Model.D_Board_User_Role_Model>();
            Boards = new List<BoardModel>();
        }

        virtual public Guid UserGuid { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>
        virtual public string UserLoginName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        virtual public string UserName { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        virtual public bool IsAdmin { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        virtual public string Languages { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Model.D_Board_User_Role_Model> Board_User_Role_Models { get; set; }

        /// <summary>
        /// 板块
        /// </summary>
        public List<BoardModel> Boards { get; set; }

    }

    public class BoardModel
    {
        /// <summary>
        /// ID
        /// </summary>
        virtual public Guid BoardGuid { get; set; }

        /// <summary>
        /// 板块名称
        /// </summary>
        virtual public string BoardName { get; set; }

    }
}