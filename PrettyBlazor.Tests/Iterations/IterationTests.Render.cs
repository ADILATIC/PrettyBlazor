﻿// ---------------------------------------------------------------
// Copyright (c) Hassan Habib All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Bunit;
using Bunit.Rendering;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Moq;
using Xunit;

namespace PrettyBlazor.Tests.Iterations
{
    public partial class IterationTests : TestContext
    {
        [Fact]
        public void ShouldHaveDefaultInitParameters()
        {
            // given . when
            var initialConditionComponent = new Iterations<object>();

            // then
            initialConditionComponent.Items.Should().BeNull();
            initialConditionComponent.Iteration.Should().BeNull();
        }

        [Fact]
        public void ShouldRenderAllItermsInList()
        {
            // given
            List<int> randomItems = CreateRandomItems();
            List<int> inputItems = randomItems;

            Type expectedIterationComponentType =
                typeof(SomeIterationComponent<int>);

            RenderFragment<int> expectedIteration = 
                CreateRenderFragment(expectedIterationComponentType);

            var componentParameters = new ComponentParameter[]
            {
                ComponentParameter.CreateParameter(
                    name: nameof(Iterations<int>.Items),
                    value: randomItems),

                ComponentParameter.CreateParameter(
                    name: nameof(Iterations<int>.Iteration),
                    value: expectedIteration)
            };

            // when
            this.renderedIterationsComponent =
               RenderComponent<Iterations<int>>(componentParameters);

            // then
            this.renderedIterationsComponent.Instance.Items
                .Should().BeEquivalentTo(randomItems);

            IReadOnlyList<IRenderedComponent<SomeIterationComponent<int>>> actualIterations = 
                this.renderedIterationsComponent.FindComponents<SomeIterationComponent<int>>();

            actualIterations.Count.Should().Be(randomItems.Count);

            actualIterations.ToList().ForEach(actualIteration =>
                actualIteration.Instance.Should().BeOfType(expectedIterationComponentType));
        }
    }
}
