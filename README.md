# Nice.DataAccess
 
 1、程序启动，创建数据库操作对象
    
    
       DataUtil.Create(new DatabaseConfig()
       {
          ConnString = Configuration["ConnString"],
          ProviderName = Configuration["ProviderName"]
       });
    
        

   
2、实体层为实体和属性添加特性(Attribute) 
    
     [Table(Name = "tbl_user_info")]        
     public class UserInfo:TEntity       
     {      
           [Id(GenerateType = IdGenerateType.Assign)]   
           public string UserId { get; set; }   
           public string UserName { get; set; }  
           ........
           [Column(IsReadOnly = true)]
           public DateTime CreateTime { get; set; }
           [Valid(1, 0)]
           [Column(Name = "nState")]
           public byte NState { get; set; }
           ........
      }          
  
  
  3、初始化DAL操作对象    
    
    private static GeneralDAL<UserInfo> dal = new GeneralDAL<UserInfo>();     
  
  4、操作实体对象
            
     public bool Insert(UserInfo entity)
     {
         return dal.Insert(entity);
     }
     public bool Update(UserInfo entity)
     {
         return dal.Update(entity);
     }
     public bool UpdateState(UserInfo entity)
     {
        return dal.Update(entity, new string[] { "NState", "ModifyTime" });
     }
     public bool Delete(string UserId)
     {
        return dal.Delete(UserId);
     }
     public bool VirtualDelete(string UserId)
     {
         return dal.VirtualDelete(UserId);
     }
     public UserInfo Get(string UserId)
     {
         return dal.Get(UserId);
     }
     public UserInfo GetByName(string UserName)
     {
        return dal.Get(o => o.UserName == UserName);
     }
     public IList<UserInfo> GetList()
     {
        return dal.GetList();
     }
     public IList<UserInfo> GetList(PageInfo page)
     {
        return dal.GetList(page);
     }         
     
