using System;
using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;
using NewsParser.DAL.Tags;
using NewsParser.BL.Exceptions;

namespace NewsParser.BL.Services.Tags
{
    public class TagDataService: ITagDataService
    {
        private readonly ITagRepository _tagRepository;

        public TagDataService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IEnumerable<Tag> GetAll()
        {
            return _tagRepository.GetAll();
        }

        public Tag GetById(int id)
        {
            return _tagRepository.GetById(id) ??
                throw new EntityNotFoundException("Tag was not found");
        }

        public Tag GetByName(string name)
        {
            return _tagRepository.GetByName(name) ??
                throw new EntityNotFoundException("Tag was not found");
        }

        public Tag Add(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag), "Tag cannot be null");
            }

            var existingTag = _tagRepository.GetByName(tag.Name);

            if (existingTag != null)
            {
                throw new BusinessLayerException($"Tag {tag.Name} already exists");
            }

            try
            {
                return _tagRepository.Add(tag);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed inserting the tag {tag.Name}", e);
            }
        }

        public void Add(List<Tag> tags)
        {
            if (tags == null || !tags.Any())
            {
                return;
            }

            try
            {
                _tagRepository.Add(tags);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed inserting a list of tags", e);
            }
        }
    }
}
