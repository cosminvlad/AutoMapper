using AutoMapper.QueryableExtensions;
using System.Linq;
using Xunit;

namespace AutoMapper.UnitTests.Projection
{
    public class ProjectExpressionCacheTest
    {
        [Fact]
        public void Should_use_cached_expression()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserModel, UserDto>()
                    .ForMember(dto => dto.FullName, opt => opt.MapFrom(src => src.LastName + " " + src.FirstName));
                cfg.CreateMap<InnerModel, InnerDto>();
            });

            var source = new UserModel[0].AsQueryable();

            var projectionExpression = new ProjectionExpression(source, config.ExpressionBuilder);
            _ = projectionExpression.To(typeof(UserDto), null, new[] { nameof(UserDto.Inner) });
            _ = projectionExpression.To(typeof(UserDto), null, new[] { nameof(UserDto.Inner) });
            _ = projectionExpression.To(typeof(UserDto), null, new[] { nameof(UserDto.Inner) });

            // TODO inspect config.ExpressionBuilder._expressionCache._dictionary
            // The count should be 1, but it's 3
        }

        public class UserModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public InnerModel Inner { get; set; }
        }

        public class UserDto
        {
            public string FullName { get; set; }
            public InnerDto Inner { get; set; }
        }

        public class InnerModel
        {
            public string Details { get; set; }

        }

        public class InnerDto
        {
            public string Details { get; set; }
        }
    }
}