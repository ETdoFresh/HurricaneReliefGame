using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace UnityPackageTemplate.Tests.Runtime
{
    public class RuntimeTemplateTest
    {
        [UnityTest]
        public IEnumerator RuntimeTemplateTestWithEnumeratorPass()
        {
            yield return null;
            Assert.IsTrue(true);
        }
    }
}
