using System;
using System.Collections.Generic;
using Atmosphere.Core.Models;
using Xunit;

namespace Atmosphere.Core.Tests;

public class ModelTests
{
    [Fact]
    public void ModifiedDateIsUpdatedAfterUpdate()
    {
        var model = User.Create("username", "password");
        var modifiedDate = model.UpdatedAt;
        model.MakeAdmin();
        Assert.NotEqual(modifiedDate, model.UpdatedAt);
    }

    [Fact]
    public void ModelsAreEqualPerGuid()
    {
        var model = User.Create("username", "password");

        Assert.Equal(model, model);
    }

    [Fact]
    public void DifferentTypesAreNotEqual()
    {
        var model1 = User.Create("username", "password");
        var model2 = new List<string>();

        Assert.NotEqual((object)model1, (object)model2);
    }

    [Fact]
    public void HashCodeGetsGenerated()
    {
        var model = User.Create("username", "password");

        Assert.NotEqual(0, model.GetHashCode());
    }
}