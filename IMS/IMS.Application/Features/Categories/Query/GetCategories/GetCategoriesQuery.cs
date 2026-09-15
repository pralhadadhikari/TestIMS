using IMS.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Categories.Query.GetCategories
{
    public class GetCategoriesQuery:IRequest<IEnumerable<CategoryInfo>>
    {
    }
}
