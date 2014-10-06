datatables-generic-paging-sorting-filtering

===========================================


      public JsonResult GetGridData([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
        //filtering
            var filterExpression = new FilterExpression<User>();

            // Apply filter to your dataset based only on the columns that actually have a search value.
            if (requestModel.Search != null && !string.IsNullOrEmpty(requestModel.Search.Value))
            {
                filterExpression.Or(x => x.Name.ToLower().StartsWith(requestModel.Search.Value));
                filterExpression.Or(x => x.Email.ToLower().StartsWith(requestModel.Search.Value));
                filterExpression.Or(x => x.Position.HasValue && x.Position.Value.ToString().ToLower().StartsWith(requestModel.Search.Value));
            }

            //sorting
            var sortExpression = new SortExpression<User>();

            // Set your dataset on the same order as requested from client-side either directly on your SQL code or easily
            // into any type or enumeration.
            var sortedColumns = requestModel.Columns.GetSortedColumns();
            foreach (var column in sortedColumns)
            {
                var direction = column.SortDirection == Column.OrderDirection.Ascendant
                          ? OrderDirection.Ascendant
                          : OrderDirection.Descendant;

                switch (column.Data)
                {
                    case "Name":
                        sortExpression.AddSortExpression(c => c.Name, direction);
                        break;
                    case "Email":
                        sortExpression.AddSortExpression(c => c.Email, direction);
                        break;
                    case "Position":
                        sortExpression.AddSortExpression(c => c.Position, direction);
                        break;
                }
            }

            //projection
            Expression<Func<User, UserView>> projection = (user) => new UserView()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Position = user.Position == null ? "" : user.Position.ToString(),
                Number = user.Number,
                Hired = user.Hired,
                IsAdmin = user.IsAdmin,
                Salary = user.Salary
            };

            var gridData = FakeDatabase.Users.ToGridData(filterExpression, projection, sortExpression, requestModel.Start, requestModel.Length);


            return Json(new DataTablesResponse(requestModel.Draw, gridData.Data, gridData.RecordsFiltered, gridData.RecordsTotal), JsonRequestBehavior.AllowGet);
        }
