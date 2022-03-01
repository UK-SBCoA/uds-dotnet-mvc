using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;

namespace UDS.Net.Web.Services
{
    public class FamilyHistoryService
    {
        private readonly UdsContext _context;
        public FamilyHistoryService(UdsContext context)
        {
            _context = context;
        }
        public async Task AddUpdateRelativeAsync(Relative relative)
        {
            switch(relative.Relation) {
                case FamilyRelationship.Father:
                    await AddUpdateFatherAsync(relative);
                    break;
                case FamilyRelationship.Mother:
                    await AddUpdateMotherAsync(relative);
                    break;
                case FamilyRelationship.Child:
                    await AddUpdateChildAsync(relative);
                    break;
                case FamilyRelationship.Sibling:
                    await AddUpdateSiblingAsync(relative);
                    break;
            }
        }
        public async Task<Relative> AddUpdateFatherAsync(Relative father)
        {
            var fatherExists = _context.Relatives.Where(x => x.Relation == FamilyRelationship.Father && x.SubjectFamilyHistoryId == father.SubjectFamilyHistoryId).Any();
            if(fatherExists) {
                _context.Relatives.Update(father);
            } else {
                _context.Relatives.Add(father);
            }
            await _context.SaveChangesAsync();
            return father;
        }
        public async Task<Relative> AddUpdateMotherAsync(Relative mother)
        {
            var motherExists = _context.Relatives.Where(x => x.Relation == FamilyRelationship.Mother && x.SubjectFamilyHistoryId == mother.SubjectFamilyHistoryId).Any();
            if (motherExists)
            {
                _context.Relatives.Update(mother);
            }
            else
            {
                _context.Relatives.Add(mother);
            }
            await _context.SaveChangesAsync();
            return mother;
        }
        public async Task<Relative> AddUpdateSiblingAsync(Relative sibling)
        {
            var siblingExists = _context.Relatives.Where(x => x.Relation == FamilyRelationship.Sibling && x.SubjectFamilyHistoryId == sibling.SubjectFamilyHistoryId).Any();
            if(siblingExists)
            {
                _context.Relatives.Update(sibling);
            } else {
                // Get Next Sibling Number
                var sibilingNextNumber = await _context.Relatives.Where(x => x.SubjectFamilyHistoryId == sibling.SubjectFamilyHistoryId && x.Relation == FamilyRelationship.Sibling).OrderByDescending(x => x.RelationshipNumber).Select(x => x.RelationshipNumber).FirstAsync();
                sibling.RelationshipNumber = sibilingNextNumber + 1;
                _context.Relatives.Add(sibling);
            }
            await _context.SaveChangesAsync();
            return sibling;
        }
        public async Task<Relative> AddUpdateChildAsync(Relative child)
        {
            var siblingExists = _context.Relatives.Where(x => x.Relation == FamilyRelationship.Child && x.SubjectFamilyHistoryId == child.SubjectFamilyHistoryId).Any();
            if (siblingExists)
            {
                _context.Relatives.Update(child);
            }
            else
            {
                // Get Next Sibling Number
                var sibilingNextNumber = await _context.Relatives.Where(x => x.SubjectFamilyHistoryId == child.SubjectFamilyHistoryId && x.Relation == FamilyRelationship.Sibling).OrderByDescending(x => x.RelationshipNumber).Select(x => x.RelationshipNumber).FirstAsync();
                child.RelationshipNumber = sibilingNextNumber + 1;
                _context.Relatives.Add(child);
            }
            await _context.SaveChangesAsync();
            return child;
        }
    }
}
