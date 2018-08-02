﻿using CoreCmd.BuildinCommands;
using CoreCmd.CommandExecution;
using CoreCmd.XunitTest.TestUtils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace AssemblyCommandExecutorTests
{
    public class AsbDummyCommand
    {
        public void FooBar1() { BasicTests.HitCounter.Hit("1"); }
        public void FooBar2(string str) { BasicTests.HitCounter.Hit("21"); }
        public void FooBar2(string str, int num) { BasicTests.HitCounter.Hit("22"); }
        public void FooBar2(int num, string str) { BasicTests.HitCounter.Hit("23"); }

        #region Foobar4
        public void FooBar4(int num) { BasicTests.HitCounter.Hit("41"); }
        public void FooBar4(double num) { BasicTests.HitCounter.Hit("42"); }
        public void FooBar4(uint num) { BasicTests.HitCounter.Hit("43"); }
        public void FooBar4(short num) { BasicTests.HitCounter.Hit("44"); }
        public void FooBar4(ushort num) { BasicTests.HitCounter.Hit("45"); }
        public void FooBar4(decimal num) { BasicTests.HitCounter.Hit("46"); }
        public void FooBar4(float num) { BasicTests.HitCounter.Hit("47"); }
        #endregion

        public void FooBar5(char p) { BasicTests.HitCounter.Hit("51"); }
        public void FooBar5(string p) { BasicTests.HitCounter.Hit("52"); }
    }

    public class BasicTests
    {
        static public HitCounter HitCounter { get; set; } = new HitCounter();
        AssemblyCommandExecutor executor = new AssemblyCommandExecutor(typeof(BasicTests));

        public BasicTests() { HitCounter.ResetDict(); }

        [Fact]
        public void Basic_usage()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar1" });
            Assert.Equal(1, HitCounter.GetHitCount("1"));
        }

        [Fact]
        public void Method_overloading_1()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar2", "hello" });
            Assert.Equal(1, HitCounter.GetHitCount("21"));
            Assert.Equal(0, HitCounter.GetHitCount("22"));
            Assert.Equal(0, HitCounter.GetHitCount("23"));
        }

        [Fact]
        public void Method_overloading_2()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar2", "hello", "123" });
            Assert.Equal(0, HitCounter.GetHitCount("1"));
            Assert.Equal(0, HitCounter.GetHitCount("21"));
            Assert.Equal(1, HitCounter.GetHitCount("22"));
            Assert.Equal(0, HitCounter.GetHitCount("23"));
        }

        [Fact]
        public void Numeric_paramter_matching_1()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar4", "4"});
            Assert.Equal(1, HitCounter.GetHitCount("41"));
            Assert.Equal(1, HitCounter.GetHitCount("42"));
            Assert.Equal(1, HitCounter.GetHitCount("43"));
            Assert.Equal(1, HitCounter.GetHitCount("44"));
            Assert.Equal(1, HitCounter.GetHitCount("45"));
            Assert.Equal(1, HitCounter.GetHitCount("46"));
            Assert.Equal(1, HitCounter.GetHitCount("47"));
        }

        [Fact]
        public void Numeric_paramter_matching_2()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar4", "4.1"});
            Assert.Equal(0, HitCounter.GetHitCount("41"));
            Assert.Equal(1, HitCounter.GetHitCount("42"));
            Assert.Equal(0, HitCounter.GetHitCount("43"));
            Assert.Equal(0, HitCounter.GetHitCount("44"));
            Assert.Equal(0, HitCounter.GetHitCount("45"));
            Assert.Equal(1, HitCounter.GetHitCount("46"));
            Assert.Equal(1, HitCounter.GetHitCount("47"));
        }

        [Fact]
        public void String_char_matching_1()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar5", "1"});
            Assert.Equal(1, HitCounter.GetHitCount("51"));  // char
            Assert.Equal(1, HitCounter.GetHitCount("52"));  // string
        }

        [Fact]
        public void String_char_matching_2()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar5", "11"});
            Assert.Equal(0, HitCounter.GetHitCount("51")); // char
            Assert.Equal(1, HitCounter.GetHitCount("52")); // string
        }
    }
}