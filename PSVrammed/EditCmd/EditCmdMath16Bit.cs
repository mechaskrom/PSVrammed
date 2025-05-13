using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using MyCustomStuff;

namespace PSVrammed
{
    partial class VramMngr
    {
        //Math on red, green and blue channels in 16 bit rgb values.
        //Special command I added for Suikoden 2, but maybe make it available to the user as an advanced edit feature?
        private class EditCmdMath16Bit : EditCmdChange
        {
            public const string SaveId = "EditCmdMath16Bit";

            //Math on alpha bit seems pointless? It can only be 0 or 1.
            //Better to use an alpha set/clear edit instead?
            private readonly Rectangle mRcPixels;
            private readonly float mValueR;
            private readonly float mValueG;
            private readonly float mValueB;
            private readonly MathOp mOp;

            public enum MathOp
            {
                Add,
                Multiply,
            }

            //TODO: Maybe add a set/assign op? Add mask channel values also and this edit command could
            //replace the old EditCmdValue16Bit class? I.e. merge the functionality of edit command
            //value and math?
            //Current math ops don't need masking. Add 0 or multiply 1 to "mask" a channel.

            //TODO: Maybe add math on other bit depths too? Not sure if useful though.
            //Probably only useful for modifying 16 bit palettes?

            public EditCmdMath16Bit(byte[] vram, Rectangle rcPixels, float r, float g, float b, MathOp op)
                : base(vram, rcPixels, BitDepth.Rgb16Bit)
            {
                mRcPixels = rcPixels;
                mValueR = r;
                mValueG = g;
                mValueB = b;
                mOp = op;
            }

            protected override void initInner()
            {
                mEcCopy.redo(); //Save old vram.
            }

            protected override void redoInner()
            {
                int vramPtr = getVramPointer16Bit(mRcPixels.Location);
                redo(mRcPixels, mVram, vramPtr, mValueR, mValueG, mValueB, mOp);
            }

            private static void redo(Rectangle rcPixels, byte[] vram, int vramPtr,
                float valR, float valG, float valB, MathOp op)
            {
                for (int y = rcPixels.Height; y > 0; y--, vramPtr += VramStride)
                {
                    for (int x = rcPixels.Width, vp = vramPtr; x > 0; x--, vp += 2)
                    {
                        int abgr = (vram[vp]) | (vram[vp + 1] << 8);
                        int r = (abgr) & 0x1F;
                        int g = (abgr >> 5) & 0x1F;
                        int b = (abgr >> 10) & 0x1F;
                        int a = abgr & MaskAlpha16Bit;

                        if (op == MathOp.Add)
                        {
                            r = (int)((r + valR) + 0.5f);
                            g = (int)((g + valG) + 0.5f);
                            b = (int)((b + valB) + 0.5f);
                        }
                        else if (op == MathOp.Multiply)
                        {
                            r = (int)((r * valR) + 0.5f);
                            g = (int)((g * valG) + 0.5f);
                            b = (int)((b * valB) + 0.5f);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                        r = r.Clamp(0, 31);
                        g = g.Clamp(0, 31);
                        b = b.Clamp(0, 31);

                        abgr = a | (b << 10) | (g << 5) | r;
                        vram[vp + 0] = (byte)(abgr);
                        vram[vp + 1] = (byte)(abgr >> 8);
                    }
                }
            }

            public override bool isRepeat(EditCmd prevEditCmd)
            {
                EditCmdMath16Bit ec = prevEditCmd as EditCmdMath16Bit;
                return ec != null && base.isRepeat(ec) &&
                    mRcPixels == ec.mRcPixels &&
                    mValueR == ec.mValueR &&
                    mValueG == ec.mValueG &&
                    mValueB == ec.mValueB &&
                    mOp == ec.mOp;
            }

            protected override string saveLine()
            {
                return FieldsWriter.toLine(SaveId, mRcPixels, mValueR, mValueG, mValueB, mOp);
            }

            public static EditCmdMath16Bit openLine(VramMngr vramMngr, FieldsReader fr)
            {
                return new EditCmdMath16Bit(vramMngr.mVram, fr.readRc(),
                    fr.readFloat(), fr.readFloat(), fr.readFloat(), fr.readEnum<MathOp>());
            }
        }
    }
}
