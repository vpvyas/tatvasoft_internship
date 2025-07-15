﻿using Microsoft.EntityFrameworkCore;
using Mission.Entities.context;
using Mission.Entities.Entities;
using Mission.Entities.Models.MissionSkillModels;
using Mission.Repositories.IRepositories;

namespace Mission.Repositories.Repositories
{
    public class MissionSkillRepository(MissionDbContext cIDbContext) : IMissionSkillRepository
    {
        private readonly MissionDbContext _cIDbContext = cIDbContext;

        public List<MissionSkillResponseModel> GetMissionSkillList()
        {
            var missionSkill = _cIDbContext.MissionSkill
                .Where(ms => !ms.IsDeleted)
                .Select(ms => new MissionSkillResponseModel()
                {
                    Id = ms.Id,
                    SkillName = ms.SkillName,
                    Status = ms.Status
                })
                .ToList();

            return missionSkill;
        }

        public MissionSkillResponseModel GetMissionSkillById(int id)
        {
            var missionSkillDetail = _cIDbContext.MissionSkill
                .Where(ms => ms.Id == id && !ms.IsDeleted)
                .Select(ms => new MissionSkillResponseModel()
                {
                    Id = ms.Id,
                    SkillName = ms.SkillName,
                    Status = ms.Status
                })
                .FirstOrDefault() ?? throw new Exception("Mission skill not found.");

            return missionSkillDetail;
        }

        public string AddMissionSkill(AddMissionSkillRequestModel model)
        {
            var skillExist = _cIDbContext.MissionSkill.Any(ms => ms.SkillName.ToLower() == model.SkillName.ToLower() && !ms.IsDeleted);

            if (skillExist) throw new Exception("Skill Name Already Exist.");

            var newSkill = new MissionSkill()
            {
                SkillName = model.SkillName,
                Status = model.Status,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            _cIDbContext.MissionSkill.Add(newSkill);
            _cIDbContext.SaveChanges();

            return "Save Skill Successfully..";
        }

        public string UpdateMissionSkill(UpdateMissionSkillRequestModel model)
        {
            var skillToUpdate = _cIDbContext.MissionSkill.FirstOrDefault(ms => ms.Id == model.Id && !ms.IsDeleted) ?? throw new Exception("Mission Skill not found.");

            bool skillAlreadyExists = _cIDbContext.MissionSkill
                .Any(ms => ms.Id != model.Id
                    && ms.SkillName.ToLower() == model.SkillName.ToLower()
                    && !ms.IsDeleted);

            if (skillAlreadyExists) throw new Exception("Skill Name Already Exist.");

            skillToUpdate.SkillName = model.SkillName;
            skillToUpdate.Status = model.Status;
            skillToUpdate.ModifiedDate = DateTime.UtcNow;

            _cIDbContext.MissionSkill.Update(skillToUpdate);
            _cIDbContext.SaveChanges();

            return "Update Mission Skill Successfully..";
        }

        public string DeleteMissionSkill(int id)
        {
            _cIDbContext.MissionSkill
                .Where(ms => ms.Id == id)
                .ExecuteUpdate(ms => ms.SetProperty(property => property.IsDeleted, true));

            return "Delete Mission Skill Successfully..";
        }
    }
}
